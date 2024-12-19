using System.Data;
using System.Text;
using Dapper;
using Npgsql;
using PhoneBook.Contracts;
using PhoneBook.Contracts.Models;
using static PhoneBook.Constants.ChildTables;

namespace PhoneBook.Repository;

public class PhoneBookRepository(IConfiguration configuration) : IPhoneBookRepository
{
    private IDbConnection GetConnection() => new NpgsqlConnection(configuration.GetConnectionString("LearnSql1"));
    
    public async Task<int> AddToChildTable(string tableName, string value)
    {
        using (IDbConnection db = GetConnection())
        {
            var sqlBuilder = new StringBuilder($"""
                                               INSERT INTO public."{tableName}" ("{Constants.ChildTables.GetField(tableName)}")
                                               VALUES (@Value)
                                               RETURNING id;
                                               """);
            
            var parameters = new DynamicParameters();
            parameters.Add("@Value", value);

            return await db.QuerySingleAsync<int>(sqlBuilder.ToString(), parameters);
        }
    }

    public async Task UpdateChildTable(string tableName, int id, string value)
    {
        using (IDbConnection db = GetConnection())
        {
            string fieldName = Constants.ChildTables.GetField(tableName);

            var sqlBuilder = new StringBuilder($"""
                                                    UPDATE public."{tableName}"
                                                    SET "{fieldName}" = @Value
                                                    WHERE id = @Id;
                                                """);

            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);
            parameters.Add("@Value", value);
            
            await db.ExecuteAsync(sqlBuilder.ToString(), parameters);
        }
    }

    public async Task DeleteFromTable(string tableName, int id)
    {
        using (IDbConnection db = GetConnection())
        {
            var sqlBuilder = new StringBuilder($"""
                                                    DELETE FROM public."{tableName}"
                                                    WHERE id = @Id;
                                                """);

            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);

            await db.ExecuteAsync(sqlBuilder.ToString(), parameters);
        }
    }

    public async Task<IEnumerable<ChildTableItem>> GetLikeFromTable(string tableName, string value)
    {
        using (IDbConnection db = GetConnection())
        {
            string fieldName = Constants.ChildTables.GetField(tableName);

            var sqlBuilder = new StringBuilder($"""
                                                    SELECT id, "{fieldName}" AS Value
                                                    FROM public."{tableName}"
                                                    WHERE "{fieldName}" ILIKE @Value
                                                    LIMIT 10;
                                                """);

            var parameters = new DynamicParameters();
            parameters.Add("@Value", $"%{value}%");

            return await db.QueryAsync<ChildTableItem>(sqlBuilder.ToString(), parameters);
        }
    }

    public Task<int> AddContact(AddOrUpdateContactRequest request)
    {
        throw new NotImplementedException();
    }

    private class IdSourcePair
    {
        public string Source { get; set; }
        public int Id { get; set; }
    }
    public async Task<int> AddOrUpdateContact(AddOrUpdateContactRequest request)
    {
        using (IDbConnection db = GetConnection())
        {
            var result = await db.QueryAsync<IdSourcePair>($"""
                SELECT id, 'first_names' AS Source FROM public.first_names WHERE name = @Name
                UNION ALL
                SELECT id, 'otchs' AS Source FROM public.otchs WHERE otch = @Otch
                UNION ALL
                SELECT id, 'surnames' AS Source FROM public.surnames WHERE surname = @Surname
                UNION ALL
                SELECT id, 'streets' AS Source FROM public.streets WHERE street = @Street;
                """, 
                new {
                    Name = request.Name, 
                    Otch = request.Otch, 
                    Surname = request.Surname, 
                    Street = request.Street 
                });

            string sql = !request.Id.HasValue
                ? @"INSERT INTO public.main (Surname_id, Name_id, Otch_id, Street_id, House, Corp, Apart, Tel)
                    VALUES (@Surname_id, @Name_id, @Otch_id, @Street_id, @House, @Corp, @Apart, @Tel)
                    RETURNING id;"
                : @"UPDATE public.main
                    SET Surname_id = @Surname_id, Name_id = @Name_id, Otch_id = @Otch_id, Street_id = @Street_id, House = @House, Corp = @Corp, Apart = @Apart, Tel = @Tel
                    WHERE Id = @Id
                    RETURNING id;";

            var parameters = new DynamicParameters();
            if(request.Id.HasValue) parameters.Add("@Id", request.Id.Value);
            parameters.Add("@Surname_id", await AddToTable(Surnames.Name, request.Surname));
            parameters.Add("@Name_id", await AddToTable(Names.Name, request.Name));
            parameters.Add("@Otch_id", await AddToTable(Otch.Name, request.Otch));
            parameters.Add("@Street_id", await AddToTable(Streets.Name, request.Street));
            parameters.Add("@House", request.House);
            parameters.Add("@Corp", request.Corp);
            parameters.Add("@Apart", request.Apart);
            parameters.Add("@Tel", request.Tel);

            return await db.QuerySingleAsync<int>(sql, parameters);


            async Task<int> AddToTable(string tableName, string value)
            {
                var pair = result.Where(r => r.Source == tableName);
                if (!pair.Any())
                    return await AddToChildTable(tableName, value);
                
                return pair.First().Id;
            };
        }
    }

    public async Task DeleteContact(int id)
    {
        using (IDbConnection db = GetConnection())
        {
            var sqlBuilder = new StringBuilder($"""
                                                    DELETE FROM public.main 
                                                    WHERE id = @Id;
                                                """);

            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);

            await db.ExecuteAsync(sqlBuilder.ToString(), parameters);

            //await DeleteFromTableWithoutLink(GetTableNames());
        }
    }

    public async Task DeleteFromTableWithoutLink(IEnumerable<string> tableNames)
    {
        using (IDbConnection db = GetConnection())
        {
            var sqlBuilder = new StringBuilder();

            foreach (var tableName in tableNames)
            {
                var mainFieldName = GetFieldInMain(tableName);
                string sql = $"""
                              DELETE FROM public."{tableName}"
                              WHERE id NOT IN (SELECT @mainFieldName FROM main);
                              """;
                var parameters = new DynamicParameters();
                parameters.Add("@mainFieldName", mainFieldName);
                await db.ExecuteAsync(sql, parameters);
            }
        }
    }

    public async Task<IEnumerable<ContactItem>> GetContacts(GetContactsRequest request)
    {
        using (IDbConnection db = GetConnection())
        {
            var sqlBuilder = new StringBuilder("""
                                               SELECT * FROM main
                                               join first_names on first_names.id=main.name_id
                                               join otchs ON otchs.id = main.otch_id
                                               join surnames ON surnames.id = main.surname_id
                                               join streets ON streets.id = main.street_id
                                               where
                                               """);
            var parameters = new DynamicParameters();
            
            if(!TryAddFilters(sqlBuilder, parameters, request.Filters))
                sqlBuilder.Replace("where", string.Empty);

            return await db.QueryAsync<ContactItem>(sqlBuilder.ToString(), parameters);
        }
    }
    
    private bool TryAddFilters(StringBuilder sqlBuilder, DynamicParameters parameters, Dictionary<string, object> filters)
    {
        Dictionary<string, Func<string, object>> filterNames = new() 
        { 
            {"name", x => x}, 
            {"surname", x => x},
            {"otch", x => x}, 
            {"street", x => x}, 
            {"house", x => x},
            {"corp", x => x},
            {"apart", x => int.Parse(x)}, 
            {"tel", x => x}
        };

        bool hasFilters = false;
        foreach (var filter in filters)
        {
            var value = filter.Value.ToString();
            if(!filterNames.TryGetValue(filter.Key, out var convertor) 
               || string.IsNullOrEmpty(value))
                continue;

            var and = hasFilters ? " and" : string.Empty;
            sqlBuilder.Append($"{and} {filter.Key} = @{filter.Key}");
            parameters.Add(filter.Key, convertor(value));
            
            hasFilters = true;
        }
        
        return hasFilters;
    }
}