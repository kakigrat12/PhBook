BEGIN;


CREATE TABLE IF NOT EXISTS public.first_names
(
    id integer NOT NULL DEFAULT nextval('first_names_id_seq'::regclass),
    name character varying(50) COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT first_names_pkey PRIMARY KEY (id),
    CONSTRAINT first_names_name_key UNIQUE (name)
    );

CREATE TABLE IF NOT EXISTS public.main
(
    id integer NOT NULL DEFAULT nextval('main_id_seq'::regclass),
    surname_id integer NOT NULL,
    name_id integer NOT NULL,
    otch_id integer,
    street_id integer,
    house character varying(50) COLLATE pg_catalog."default",
    corp character varying(50) COLLATE pg_catalog."default",
    apart integer,
    tel character varying(50) COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT main_pkey PRIMARY KEY (id)
    );

CREATE TABLE IF NOT EXISTS public.otchs
(
    id integer NOT NULL DEFAULT nextval('otchs_id_seq'::regclass),
    otch character varying(50) COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT otchs_pkey PRIMARY KEY (id),
    CONSTRAINT otchs_otch_key UNIQUE (otch)
    );

CREATE TABLE IF NOT EXISTS public.streets
(
    id integer NOT NULL DEFAULT nextval('streets_id_seq'::regclass),
    street character varying(50) COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT streets_pkey PRIMARY KEY (id),
    CONSTRAINT streets_street_key UNIQUE (street)
    );

CREATE TABLE IF NOT EXISTS public.surnames
(
    id integer NOT NULL DEFAULT nextval('surnames_id_seq'::regclass),
    surname character varying(50) COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT surnames_pkey PRIMARY KEY (id),
    CONSTRAINT surnames_surname_key UNIQUE (surname)
    );

ALTER TABLE IF EXISTS public.main
    ADD CONSTRAINT main_name_id_fkey FOREIGN KEY (name_id)
    REFERENCES public.first_names (id) MATCH SIMPLE
    ON UPDATE NO ACTION
       ON DELETE NO ACTION;


ALTER TABLE IF EXISTS public.main
    ADD CONSTRAINT main_otch_id_fkey FOREIGN KEY (otch_id)
    REFERENCES public.otchs (id) MATCH SIMPLE
    ON UPDATE NO ACTION
       ON DELETE NO ACTION;


ALTER TABLE IF EXISTS public.main
    ADD CONSTRAINT main_street_id_fkey FOREIGN KEY (street_id)
    REFERENCES public.streets (id) MATCH SIMPLE
    ON UPDATE NO ACTION
       ON DELETE NO ACTION;


ALTER TABLE IF EXISTS public.main
    ADD CONSTRAINT main_surname_id_fkey FOREIGN KEY (surname_id)
    REFERENCES public.surnames (id) MATCH SIMPLE
    ON UPDATE NO ACTION
       ON DELETE NO ACTION;

END;