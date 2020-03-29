--
-- PostgreSQL database dump
--

-- Dumped from database version 10.3
-- Dumped by pg_dump version 10.3

-- Started on 2018-06-08 10:46:42

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 238 (class 1255 OID 32971)
-- Name: INIT(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public."INIT"() RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
INSERT INTO public."OBTIZNOST"(
	"RADKY", "SLOUPCE", "POCETMIN", "NAZEV")
	VALUES (9, 9, 10, 'Zacatecnik'), (16, 16, 40, 'Pokrocily'), (16, 30, 99, 'Expert');
	
INSERT INTO public."OMEZENI"(
	"MIN", "MAX", "POCETMINPROC")
	VALUES (9, 100, 40);
	
INSERT INTO public."STAV"("STAV")
	VALUES ('Rozehrana'), ('Uspesne ukoncena'), ('Neuspesne ukoncena');
	END;
$$;


ALTER FUNCTION public."INIT"() OWNER TO postgres;

--
-- TOC entry 246 (class 1255 OID 32941)
-- Name: MNOHO_MIN(integer); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public."MNOHO_MIN"(oblast integer) RETURNS integer
    LANGUAGE plpgsql
    AS $$DECLARE
pocet_min integer;
pocet_oznacenych integer;

BEGIN

SELECT "OBTIZNOST"."POCET_MIN" INTO pocet_min FROM "OBLAST" INNER JOIN "OBTIZNOST" ON "OBTIZNOST"."ID" = "OBLAST"."OBTIZNOST" WHERE "OBLAST"."ID" = oblast;
SELECT count(*) INTO pocet_oznacenych FROM "POLE" WHERE "OBLAST" = oblast AND "STAV" = -1;

IF pocet_min =< pocet_oznacenych  THEN
	RETURN 1;
ELSE
	RETURN 0;
END IF;

END;
$$;


ALTER FUNCTION public."MNOHO_MIN"(oblast integer) OWNER TO postgres;

--
-- TOC entry 247 (class 1255 OID 32964)
-- Name: ODKRYJ_POLE(integer, integer, integer); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public."ODKRYJ_POLE"(oblast integer, x integer, y integer) RETURNS void
    LANGUAGE plpgsql
    AS $$DECLARE
val integer;

BEGIN
  SELECT count(*) INTO val FROM "POLE" WHERE "OBLAST" = oblast AND "X" = x AND "Y" = y AND "STAV" = 0;
  IF val > 0  THEN

	UPDATE "POLE" SET "STAV"=1 WHERE "OBLAST" = oblast AND "STAV" = 0 AND "X" = x AND "Y" = y;
  
  	SELECT "VAL" INTO val FROM "POLE" WHERE "OBLAST" = oblast AND "X" = x AND "Y" = y;
	IF val = 0 THEN 
		PERFORM ODKRYJ_POLE(oblast, x-1, y-1);
		PERFORM ODKRYJ_POLE(oblast, x-1, y);
		PERFORM ODKRYJ_POLE(oblast, x-1, y+1);
		PERFORM ODKRYJ_POLE(oblast, x, y-1);
		PERFORM ODKRYJ_POLE(oblast, x, y+1);
		PERFORM ODKRYJ_POLE(oblast, x+1, y-1);
		PERFORM ODKRYJ_POLE(oblast, x+1, y);
		PERFORM ODKRYJ_POLE(oblast, x+1, y+1);
	ELSE IF val = -1 THEN
		raise notice 'Odkryta mina, prohral jste';
		PERFORM OZNAC_MINY(oblast);
		UPDATE "HRA" SET "STAV"=3;
	END IF;
	END IF;
	
	PERFORM VYHRA(oblast);
  END IF;
END;$$;


ALTER FUNCTION public."ODKRYJ_POLE"(oblast integer, x integer, y integer) OWNER TO postgres;

--
-- TOC entry 224 (class 1255 OID 32939)
-- Name: ODKRYTA_MINA(integer, integer, integer); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public."ODKRYTA_MINA"(oblast integer, x integer, y integer) RETURNS integer
    LANGUAGE plpgsql
    AS $$
DECLARE
val integer;

BEGIN

SELECT "VAL" INTO val FROM "POLE" WHERE "OBLAST" = oblast AND "X" = x AND "Y" = y;

IF val = -1 THEN
	RETURN 1;
ELSE
	RETURN 0;
END IF;

END;
$$;


ALTER FUNCTION public."ODKRYTA_MINA"(oblast integer, x integer, y integer) OWNER TO postgres;

--
-- TOC entry 225 (class 1255 OID 32966)
-- Name: OZNAC_MINU(integer, integer, integer); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public."OZNAC_MINU"(oblast integer, x integer, y integer) RETURNS void
    LANGUAGE plpgsql
    AS $$DECLARE
res integer;

BEGIN
	select MNOHO_MIN(oblast) into res;
	if res = 1 then
		raise exception 'Mnoho min';
	end if;

	INSERT INTO "MINA"("POLE") SELECT "POLE"."ID" FROM "POLE" WHERE "POLE"."OBLAST" = oblast AND "POLE"."X" = x AND "POLE"."Y" = y  AND "STAV" = 0;
	UPDATE "POLE" SET "STAV" = -1 WHERE "OBLAST" = oblast AND "X" = x AND "Y" = y AND "STAV" = 0;

END;
$$;


ALTER FUNCTION public."OZNAC_MINU"(oblast integer, x integer, y integer) OWNER TO postgres;

--
-- TOC entry 248 (class 1255 OID 32948)
-- Name: OZNAC_MINY(integer); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public."OZNAC_MINY"(oblast integer) RETURNS void
    LANGUAGE plpgsql
    AS $$BEGIN

	INSERT INTO "MINA"("POLE") SELECT "POLE"."ID" FROM "POLE" WHERE "POLE"."OBLAST" = oblast AND "STAV" = 0 AND "VAL" = -1 AND NOT EXISTS (SELECT * FROM "MINA" INNER JOIN "POLE" ON "MINA"."POLE" = "POLE"."ID");
	UPDATE "POLE" SET "STAV" = -2 WHERE "OBLAST" = oblast AND "STAV" = -1 AND "VAL" != -1;
	UPDATE "POLE" SET "STAV" = -1 WHERE "OBLAST" = oblast AND "STAV" = 0 AND "VAL" = -1;

END;
$$;


ALTER FUNCTION public."OZNAC_MINY"(oblast integer) OWNER TO postgres;

--
-- TOC entry 243 (class 1255 OID 32927)
-- Name: RADEK_OBLASTI(integer, integer); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public."RADEK_OBLASTI"(oblast integer, y integer) RETURNS character varying
    LANGUAGE plpgsql
    AS $$DECLARE
res character varying(256);
row "POLE"%ROWTYPE;

BEGIN
  res:= '|';
  FOR row IN (SELECT * FROM "POLE" WHERE "Y" = y AND "OBLAST" = oblast ORDER BY "X")
	LOOP
    
    if row."STAV" = 0 then
      res := res || '?|';
    elsif row."STAV" = -1 then
      res := res || '!|';
    elsif row."STAV" = 1 and row."VAL" = '0' then
      res := res || ' |';
    else
      res := res || row."VAL" || '|';
    end if;
  END LOOP;
  
  RETURN res;
  
END;$$;


ALTER FUNCTION public."RADEK_OBLASTI"(oblast integer, y integer) OWNER TO postgres;

--
-- TOC entry 218 (class 1255 OID 32900)
-- Name: SPATNY_PARAMETR(integer, integer, integer); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public."SPATNY_PARAMETR"(radky integer, sloupce integer, pocet_min integer) RETURNS integer
    LANGUAGE plpgsql
    AS $$DECLARE
min	integer;
max	integer;
pocet_min_proc integer;

BEGIN

  SELECT "MIN", "MAX", "POCET_MIN_PROC" INTO min, max, pocet_min_proc
	FROM public."OMEZENI";
	
IF (radky >= min AND radky <= max) AND (sloupce >= min AND sloupce <= max) AND pocet_min <= (radky * sloupce * pocet_min_proc / 100) THEN
RETURN 0;
ELSE
RETURN 1;
END IF;

END;$$;


ALTER FUNCTION public."SPATNY_PARAMETR"(radky integer, sloupce integer, pocet_min integer) OWNER TO postgres;

--
-- TOC entry 242 (class 1255 OID 32946)
-- Name: SPOCITEJ_OBLAST(integer); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public."SPOCITEJ_OBLAST"(oblast integer) RETURNS void
    LANGUAGE plpgsql
    AS $$DECLARE
pocet_min integer;
x_count integer;
y_count integer;
x integer;
y integer;
val integer;
mine_count integer;

BEGIN

SELECT "OBTIZNOST"."POCET_MIN", "OBTIZNOST"."SLOUPCE", "OBTIZNOST"."RADKY"  INTO pocet_min, x_count, y_count FROM "OBLAST" INNER JOIN "OBTIZNOST" ON "OBTIZNOST"."ID" = "OBLAST"."OBTIZNOST" WHERE "OBLAST"."ID" = oblast;

FOR x IN 1..x_count LOOP
  FOR y IN 1..y_count LOOP
  
	SELECT count(*) INTO val FROM "POLE" WHERE "OBLAST" = oblast AND "X" = x AND "Y" = y;
    CONTINUE WHEN val > 0;
	
	mine_count := 0;
	  
	SELECT "VAL" INTO val FROM "POLE" WHERE "OBLAST" = oblast AND "X" = x-1 AND "Y" = y-1;
	IF val = -1 THEN mine_count := mine_count +1; END IF;
	
	SELECT "VAL" INTO val FROM "POLE" WHERE "OBLAST" = oblast AND "X" = x-1 AND "Y" = y;
	IF val = -1 THEN mine_count := mine_count +1; END IF;
	
	SELECT "VAL" INTO val FROM "POLE" WHERE "OBLAST" = oblast AND "X" = x-1 AND "Y" = y+1;
	IF val = -1 THEN mine_count := mine_count +1; END IF;
	
	SELECT "VAL" INTO val FROM "POLE" WHERE "OBLAST" = oblast AND "X" = x AND "Y" = y-1;
	IF val = -1 THEN mine_count := mine_count +1; END IF;
	
	SELECT "VAL" INTO val FROM "POLE" WHERE "OBLAST" = oblast AND "X" = x AND "Y" = y+1;
	IF val = -1 THEN mine_count := mine_count +1; END IF;
	
	SELECT "VAL" INTO val FROM "POLE" WHERE "OBLAST" = oblast AND "X" = x+1 AND "Y" = y-1;
	IF val = -1 THEN mine_count := mine_count +1; END IF;
	
	SELECT "VAL" INTO val FROM "POLE" WHERE "OBLAST" = oblast AND "X" = x+1 AND "Y" = y;
	IF val = -1 THEN mine_count := mine_count +1; END IF;
	
	SELECT "VAL" INTO val FROM "POLE" WHERE "OBLAST" = oblast AND "X" = x+1 AND "Y" = y+1;
	IF val = -1 THEN mine_count := mine_count +1; END IF;
    
    INSERT INTO "POLE" ("OBLAST", "X", "Y", "VAL", "STAV") VALUES (oblast, x, y, mine_count, 0);
	
  END LOOP;
END LOOP;

END;$$;


ALTER FUNCTION public."SPOCITEJ_OBLAST"(oblast integer) OWNER TO postgres;

--
-- TOC entry 229 (class 1255 OID 32984)
-- Name: TG_END_GAME(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public."TG_END_GAME"() RETURNS trigger
    LANGUAGE plpgsql
    AS $$DECLARE
res integer;

BEGIN


SELECT COUNT(*) INTO res FROM "HRA" WHERE "HRA"."OBLAST" = NEW."OBLAST" AND "HRA"."STAV" != 1;
IF res > 0 THEN
    RAISE EXCEPTION 'HRA BYLA UKONCENA';
  END IF;

  
  RETURN NEW;

END;
$$;


ALTER FUNCTION public."TG_END_GAME"() OWNER TO postgres;

--
-- TOC entry 241 (class 1255 OID 33019)
-- Name: TG_FIRSTMOVE(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public."TG_FIRSTMOVE"() RETURNS trigger
    LANGUAGE plpgsql
    AS $$DECLARE
res integer;

BEGIN

SELECT "FIRSTMOVE" INTO res FROM "HRA" WHERE "HRA"."ID" = NEW."HRA";
IF res = NULL then
UPDATE "HRA" SET "FIRSTMOVE" = NEW."TIMESTAMP" WHERE "HRA"."ID" = NEW."HRA";
end if;


RETURN NEW;
END;
$$;


ALTER FUNCTION public."TG_FIRSTMOVE"() OWNER TO postgres;

--
-- TOC entry 250 (class 1255 OID 32978)
-- Name: TG_NEW_GAME(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public."TG_NEW_GAME"() RETURNS trigger
    LANGUAGE plpgsql
    AS $$BEGIN
PERFORM ZAMINUJ_OBLAST(NEW."ID");
PERFORM SPOCITEJ_OBLAST(NEW."ID");

INSERT INTO public."HRA"(
	"STAV", "OBLAST")
	VALUES (1, NEW."ID");

return NEW;
END;$$;


ALTER FUNCTION public."TG_NEW_GAME"() OWNER TO postgres;

--
-- TOC entry 235 (class 1255 OID 32975)
-- Name: TG_OBTIZNOST(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public."TG_OBTIZNOST"() RETURNS trigger
    LANGUAGE plpgsql
    AS $$DECLARE
res integer;

BEGIN

SELECT SPATNY_PARAMETR(NEW."RADKY", NEW."SLOUPCE", NEW."POCET_MIN") INTO res;

IF res = 1 THEN
    RAISE EXCEPTION 'Chybny parametr';
  END IF;
  
  RETURN NEW;

END;$$;


ALTER FUNCTION public."TG_OBTIZNOST"() OWNER TO postgres;

--
-- TOC entry 233 (class 1255 OID 32980)
-- Name: TG_ODKRYJ(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public."TG_ODKRYJ"() RETURNS trigger
    LANGUAGE plpgsql
    AS $$DECLARE
res integer;

BEGIN
if NEW."STAV" = 1 then
SELECT COUNT(*) INTO res FROM "POLE" WHERE "OBLAST" = NEW."OBLAST" AND "X" = NEW."X" AND "Y" = NEW."Y" AND OLD."STAV" = 1;
IF res > 0 THEN
    RAISE EXCEPTION 'Jiz odkryto';
  END IF;
  
 INSERT INTO public."TAH"("POLE", "TIMESTAMP")
	VALUES (NEW."ID", now());
 UPDATE public."HRA"
	SET "LASTMOVE"=now()
	WHERE "OBLAST" = NEW."OBLAST";
	
SELECT ODKRYTA_MINA(NEW."OBLAST", NEW."X", NEW."Y") INTO res;
IF res = 1 THEN
	RAISE NOTICE 'PROHRALI JSTE!';
	UPDATE public."HRA" SET "STAV"=3 WHERE "HRA"."OBLAST" = NEW."OBLAST";
	--PERFORM OZNAC_MINY(NEW."OBLAST");
	RETURN NEW;
END IF;
end if;
	
PERFORM ODKRYJ_POLE(NEW."OBLAST", NEW."X", NEW."Y");
RETURN NEW;
END;
$$;


ALTER FUNCTION public."TG_ODKRYJ"() OWNER TO postgres;

--
-- TOC entry 239 (class 1255 OID 32981)
-- Name: TG_OZNAC(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public."TG_OZNAC"() RETURNS trigger
    LANGUAGE plpgsql
    AS $$DECLARE
res integer;

BEGIN
if NEW."STAV" = -1 then

SELECT COUNT(*) INTO res FROM "POLE" WHERE "OBLAST" = NEW."OBLAST" AND "X" = NEW."X" AND "Y" = NEW."Y" AND OLD."STAV" = -1;
IF res > 0 THEN
    RAISE EXCEPTION 'Pole jiz OZNACENO';
  END IF;
  
SELECT MNOHO_MIN(NEW."OBLAST") INTO res;

IF res = 1 THEN
    RAISE NOTICE 'Jiz OZNACENO prilis mnoho min';
  END IF;
  
  INSERT INTO public."TAH"("POLE", "TIMESTAMP")
	VALUES (NEW."ID", now());
	
	UPDATE public."HRA"
	SET "LASTMOVE"=now()
	WHERE "OBLAST" = NEW."OBLAST";
	
end if;

RETURN NEW;
END;
$$;


ALTER FUNCTION public."TG_OZNAC"() OWNER TO postgres;

--
-- TOC entry 245 (class 1255 OID 33010)
-- Name: TG_PROHRA(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public."TG_PROHRA"() RETURNS trigger
    LANGUAGE plpgsql
    AS $$DECLARE
res integer;

BEGIN

select count(*) into res FROM "HRA" WHERE "HRA"."OBLAST" = NEW."OBLAST" AND "HRA"."STAV" !=1;
if res >1 then 
perform OZNAC_MINY(NEW."OBLAST"); end if;

return NEW;

END;
$$;


ALTER FUNCTION public."TG_PROHRA"() OWNER TO postgres;

--
-- TOC entry 223 (class 1255 OID 33031)
-- Name: TG_TAH(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public."TG_TAH"() RETURNS trigger
    LANGUAGE plpgsql
    AS $$DECLARE
res integer;
t timestamp with time zone;

BEGIN

SELECT COUNT(*) INTO res FROM "HRA" WHERE "HRA"."OBLAST" = NEW."OBLAST" AND "HRA"."STAV" != 1;
IF res > 0 THEN
    RAISE EXCEPTION 'HRA BYLA UKONCENA';
  END IF;
  
SELECT "STAV" INTO res FROM "POLE" WHERE "OBLAST" = NEW."OBLAST" AND "X" = NEW."X" AND "Y" = NEW."Y";
	if res = 1 then
		raise exception 'Pole jiz odkryto';
	else if res = -1 AND NEW."STAV" != 0 then
		raise exception 'Pole oznaceno jako zaminovane';
	end if; end if;

if NEW."STAV" = 1 then
	perform odkryj_pole(NEW."OBLAST", NEW."X", NEW."Y");
else if NEW."STAV" = -1 then
	perform oznac_minu(NEW."OBLAST", NEW."X", NEW."Y");
else
	perform zrus_minu(NEW."OBLAST", NEW."X", NEW."Y");
end if; end if;

UPDATE public."HRA" SET "LASTMOVE"=now() WHERE "OBLAST" = NEW."OBLAST";
SELECT "FIRSTMOVE" INTO t FROM "HRA" WHERE "HRA"."OBLAST" = NEW."OBLAST";
IF t IS NULL then
UPDATE "HRA" SET "FIRSTMOVE" = now() WHERE "HRA"."OBLAST" = NEW."OBLAST";
end if;

SELECT "ID" INTO NEW."POLE" from "POLE" where "OBLAST" = NEW."OBLAST" and "X" = NEW."X" and "Y"= NEW."Y";
select now() into NEW."TIMESTAMP";
return NEW;

END;
$$;


ALTER FUNCTION public."TG_TAH"() OWNER TO postgres;

--
-- TOC entry 226 (class 1255 OID 32986)
-- Name: TG_VYHRA(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public."TG_VYHRA"() RETURNS trigger
    LANGUAGE plpgsql
    AS $$DECLARE
res integer;

BEGIN

SELECT VYHRA(NEW."OBLAST") INTO res;
IF res = 1 THEN
	RAISE NOTICE 'GRATULUJEME, VYHRAL JSTE!';
	UPDATE public."HRA" SET "STAV"=2 WHERE "OBLAST" = NEW."OBLAST";
	--PERFORM OZNAC_MINY(NEW."OBLAST");
	RETURN NEW;
END IF;
RETURN NEW;
END;
	
$$;


ALTER FUNCTION public."TG_VYHRA"() OWNER TO postgres;

--
-- TOC entry 230 (class 1255 OID 32918)
-- Name: VYHRA(integer); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public."VYHRA"(oblast integer) RETURNS integer
    LANGUAGE plpgsql
    AS $$DECLARE
count	integer;
pocet_min	integer;

BEGIN
  SELECT count(*) INTO count FROM "POLE" WHERE "POLE"."OBLAST" = oblast AND ("POLE"."STAV" = 0 OR "POLE"."STAV" = -1 OR "POLE"."STAV" = null);
  SELECT "OBTIZNOST"."POCET_MIN" INTO pocet_min FROM "OBTIZNOST"
  INNER JOIN "OBLAST" ON "OBLAST"."OBTIZNOST" = "OBTIZNOST"."ID"
  WHERE "OBLAST"."ID" = oblast ;
  
  IF count = pocet_min THEN
  	RAISE NOTICE 'GRATULUJEME, VYHRAL JSTE!';
	UPDATE public."HRA" SET "STAV"=2 WHERE "OBLAST" = oblast;
	PERFORM OZNAC_MINY(oblast);
    RETURN 1;
  ELSE
	RETURN 0;
  END IF;
END;

$$;


ALTER FUNCTION public."VYHRA"(oblast integer) OWNER TO postgres;

--
-- TOC entry 228 (class 1255 OID 32944)
-- Name: ZAMINUJ_OBLAST(integer); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public."ZAMINUJ_OBLAST"(oblast integer) RETURNS void
    LANGUAGE plpgsql
    AS $$DECLARE
pocet_min integer;
x_count integer;
y_count integer;
x integer;
y integer;
mina integer;

BEGIN

SELECT "OBTIZNOST"."POCET_MIN", "OBTIZNOST"."SLOUPCE", "OBTIZNOST"."RADKY"  INTO pocet_min, x_count, y_count FROM "OBLAST" INNER JOIN "OBTIZNOST" ON "OBTIZNOST"."ID" = "OBLAST"."OBTIZNOST" WHERE "OBLAST"."ID" = oblast;

FOR i IN 1..pocet_min LOOP
    LOOP
      SELECT random_between(1, x_count), random_between(1, y_count) INTO x, y;
      SELECT COUNT(*) INTO mina FROM "POLE" WHERE "OBLAST" = oblast AND "X" = x AND "Y" = y;
      EXIT WHEN mina = 0;
    END LOOP;
    
    INSERT INTO "POLE" ("OBLAST", "X", "Y", "VAL", "STAV") VALUES (oblast, x, y, -1, 0);
  END LOOP;

END;$$;


ALTER FUNCTION public."ZAMINUJ_OBLAST"(oblast integer) OWNER TO postgres;

--
-- TOC entry 231 (class 1255 OID 32967)
-- Name: ZRUS_MINU(integer, integer, integer); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public."ZRUS_MINU"(oblast integer, x integer, y integer) RETURNS void
    LANGUAGE plpgsql
    AS $$BEGIN

	DELETE FROM "MINA" WHERE "MINA"."POLE" = (SELECT "POLE"."ID" FROM "POLE" WHERE "POLE"."OBLAST" = oblast AND "POLE"."X" = x AND "POLE"."Y" = y AND "POLE"."STAV" = 0);
	UPDATE "POLE" SET "STAV" = 0 WHERE "OBLAST" = oblast AND "X" = x AND "Y" = y AND "STAV" = -1;

END;
$$;


ALTER FUNCTION public."ZRUS_MINU"(oblast integer, x integer, y integer) OWNER TO postgres;

--
-- TOC entry 237 (class 1255 OID 33038)
-- Name: radek_oblasti(integer, integer); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.radek_oblasti(oblast integer, y integer) RETURNS character varying
    LANGUAGE plpgsql
    AS $$
DECLARE
res character varying(256);
row "POLE"%ROWTYPE;

BEGIN
  res:= '|';
  FOR row IN (SELECT * FROM "POLE" WHERE "Y" = y AND "OBLAST" = oblast ORDER BY "X")
	LOOP
    
    if row."STAV" = 0 then
      res := res || '?|';
    elsif row."STAV" = -1 then
      res := res || '!|';
    elsif row."STAV" = 1 and row."VAL" = '0' then
      res := res || ' |';
    else
      res := res || row."VAL" || '|';
    end if;
  END LOOP;
  
  RETURN res;
  
END;
$$;


ALTER FUNCTION public.radek_oblasti(oblast integer, y integer) OWNER TO postgres;

SET default_tablespace = '';

SET default_with_oids = false;

--
-- TOC entry 204 (class 1259 OID 16430)
-- Name: MINA; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."MINA" (
    "ID" integer NOT NULL,
    "POLE" integer NOT NULL
);


ALTER TABLE public."MINA" OWNER TO postgres;

--
-- TOC entry 202 (class 1259 OID 16422)
-- Name: POLE; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."POLE" (
    "ID" integer NOT NULL,
    "X" integer NOT NULL,
    "Y" integer NOT NULL,
    "VAL" integer NOT NULL,
    "OBLAST" integer,
    "STAV" integer NOT NULL
);


ALTER TABLE public."POLE" OWNER TO postgres;

--
-- TOC entry 2918 (class 0 OID 0)
-- Dependencies: 202
-- Name: TABLE "POLE"; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON TABLE public."POLE" IS 'VAL:
1-8 pocet min
0 nesousedi s minou (nebo null)
-1 mina

STAV:
0 default (nebo null)
1 odkryta
-1 oznacena mina
-2 spatne oznacena mina';


--
-- TOC entry 213 (class 1259 OID 16517)
-- Name: CHYBNE_MINY; Type: VIEW; Schema: public; Owner: postgres
--

CREATE VIEW public."CHYBNE_MINY" AS
 SELECT "MINA"."POLE",
    "POLE"."OBLAST",
    "POLE"."X",
    "POLE"."Y"
   FROM (public."MINA"
     JOIN public."POLE" ON (("MINA"."POLE" = "POLE"."ID")))
  WHERE ("POLE"."VAL" <> '-1'::integer)
  ORDER BY "POLE"."OBLAST";


ALTER TABLE public."CHYBNE_MINY" OWNER TO postgres;

--
-- TOC entry 212 (class 1259 OID 16491)
-- Name: HRA; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."HRA" (
    "ID" integer NOT NULL,
    "STAV" integer NOT NULL,
    "POCETMIN" integer,
    "OBLAST" integer,
    "FIRSTMOVE" timestamp(6) with time zone,
    "LASTMOVE" timestamp(6) with time zone
);


ALTER TABLE public."HRA" OWNER TO postgres;

--
-- TOC entry 211 (class 1259 OID 16489)
-- Name: HRA_ID_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."HRA_ID_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."HRA_ID_seq" OWNER TO postgres;

--
-- TOC entry 2919 (class 0 OID 0)
-- Dependencies: 211
-- Name: HRA_ID_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."HRA_ID_seq" OWNED BY public."HRA"."ID";


--
-- TOC entry 203 (class 1259 OID 16428)
-- Name: MINA_ID_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."MINA_ID_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."MINA_ID_seq" OWNER TO postgres;

--
-- TOC entry 2920 (class 0 OID 0)
-- Dependencies: 203
-- Name: MINA_ID_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."MINA_ID_seq" OWNED BY public."MINA"."ID";


--
-- TOC entry 210 (class 1259 OID 16475)
-- Name: OBLAST; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."OBLAST" (
    "ID" integer NOT NULL,
    "OBTIZNOST" integer NOT NULL
);


ALTER TABLE public."OBLAST" OWNER TO postgres;

--
-- TOC entry 209 (class 1259 OID 16473)
-- Name: OBLAST_ID_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."OBLAST_ID_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."OBLAST_ID_seq" OWNER TO postgres;

--
-- TOC entry 2921 (class 0 OID 0)
-- Dependencies: 209
-- Name: OBLAST_ID_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."OBLAST_ID_seq" OWNED BY public."OBLAST"."ID";


--
-- TOC entry 216 (class 1259 OID 33039)
-- Name: OBLAST_TISK; Type: VIEW; Schema: public; Owner: postgres
--

CREATE VIEW public."OBLAST_TISK" AS
 SELECT DISTINCT "POLE"."OBLAST",
    "POLE"."Y" AS radek,
    public.radek_oblasti("POLE"."OBLAST", "POLE"."Y") AS radek_oblasti
   FROM public."POLE"
  ORDER BY "POLE"."OBLAST", "POLE"."Y";


ALTER TABLE public."OBLAST_TISK" OWNER TO postgres;

--
-- TOC entry 198 (class 1259 OID 16400)
-- Name: OBTIZNOST; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."OBTIZNOST" (
    "RADKY" integer NOT NULL,
    "SLOUPCE" integer NOT NULL,
    "POCET_MIN" integer NOT NULL,
    "ID" integer NOT NULL,
    "NAZEV" character varying(60)
);


ALTER TABLE public."OBTIZNOST" OWNER TO postgres;

--
-- TOC entry 197 (class 1259 OID 16398)
-- Name: OBTIZNOST_ID_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."OBTIZNOST_ID_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."OBTIZNOST_ID_seq" OWNER TO postgres;

--
-- TOC entry 2922 (class 0 OID 0)
-- Dependencies: 197
-- Name: OBTIZNOST_ID_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."OBTIZNOST_ID_seq" OWNED BY public."OBTIZNOST"."ID";


--
-- TOC entry 206 (class 1259 OID 16443)
-- Name: OMEZENI; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."OMEZENI" (
    "ID" integer NOT NULL,
    "MIN" integer NOT NULL,
    "MAX" integer NOT NULL,
    "POCET_MIN_PROC" integer NOT NULL
);


ALTER TABLE public."OMEZENI" OWNER TO postgres;

--
-- TOC entry 205 (class 1259 OID 16441)
-- Name: OMEZENI_ID_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."OMEZENI_ID_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."OMEZENI_ID_seq" OWNER TO postgres;

--
-- TOC entry 2923 (class 0 OID 0)
-- Dependencies: 205
-- Name: OMEZENI_ID_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."OMEZENI_ID_seq" OWNED BY public."OMEZENI"."ID";


--
-- TOC entry 201 (class 1259 OID 16420)
-- Name: POLE_ID_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."POLE_ID_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."POLE_ID_seq" OWNER TO postgres;

--
-- TOC entry 2924 (class 0 OID 0)
-- Dependencies: 201
-- Name: POLE_ID_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."POLE_ID_seq" OWNED BY public."POLE"."ID";


--
-- TOC entry 200 (class 1259 OID 16411)
-- Name: STAV; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."STAV" (
    "ID" integer NOT NULL,
    "STAV" character varying(60) NOT NULL
);


ALTER TABLE public."STAV" OWNER TO postgres;

--
-- TOC entry 215 (class 1259 OID 33033)
-- Name: PORAZENI; Type: VIEW; Schema: public; Owner: postgres
--

CREATE VIEW public."PORAZENI" AS
 SELECT "HRA"."ID",
    "STAV"."STAV",
    "OBTIZNOST"."NAZEV",
    "OBTIZNOST"."RADKY",
    "OBTIZNOST"."SLOUPCE",
    "OBTIZNOST"."POCET_MIN",
    ("HRA"."LASTMOVE" - "HRA"."FIRSTMOVE")
   FROM (((((public."HRA"
     JOIN public."OBLAST" ON (("HRA"."OBLAST" = "OBLAST"."ID")))
     JOIN public."STAV" ON (("HRA"."STAV" = "STAV"."ID")))
     JOIN public."OBTIZNOST" ON (("OBLAST"."OBTIZNOST" = "OBTIZNOST"."ID")))
     JOIN public."POLE" ON (("POLE"."OBLAST" = "HRA"."OBLAST")))
     JOIN public."MINA" ON (("MINA"."POLE" = "POLE"."ID")))
  WHERE ("STAV"."ID" = 3);


ALTER TABLE public."PORAZENI" OWNER TO postgres;

--
-- TOC entry 199 (class 1259 OID 16409)
-- Name: STAV_ID_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."STAV_ID_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."STAV_ID_seq" OWNER TO postgres;

--
-- TOC entry 2925 (class 0 OID 0)
-- Dependencies: 199
-- Name: STAV_ID_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."STAV_ID_seq" OWNED BY public."STAV"."ID";


--
-- TOC entry 208 (class 1259 OID 16451)
-- Name: TAH; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."TAH" (
    "ID" integer NOT NULL,
    "POLE" integer NOT NULL,
    "TIMESTAMP" timestamp(6) with time zone NOT NULL,
    "OBLAST" integer,
    "X" integer,
    "Y" integer,
    "STAV" integer
);


ALTER TABLE public."TAH" OWNER TO postgres;

--
-- TOC entry 207 (class 1259 OID 16449)
-- Name: TAH_ID_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."TAH_ID_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."TAH_ID_seq" OWNER TO postgres;

--
-- TOC entry 2926 (class 0 OID 0)
-- Dependencies: 207
-- Name: TAH_ID_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."TAH_ID_seq" OWNED BY public."TAH"."ID";


--
-- TOC entry 214 (class 1259 OID 33000)
-- Name: VITEZOVE; Type: VIEW; Schema: public; Owner: postgres
--

CREATE VIEW public."VITEZOVE" AS
 SELECT "HRA"."ID",
    "STAV"."STAV",
    "OBTIZNOST"."NAZEV",
    "OBTIZNOST"."RADKY",
    "OBTIZNOST"."SLOUPCE",
    "OBTIZNOST"."POCET_MIN",
    ("HRA"."LASTMOVE" - "HRA"."FIRSTMOVE")
   FROM (((public."HRA"
     JOIN public."OBLAST" ON (("HRA"."OBLAST" = "OBLAST"."ID")))
     JOIN public."STAV" ON (("HRA"."STAV" = "STAV"."ID")))
     JOIN public."OBTIZNOST" ON (("OBLAST"."OBTIZNOST" = "OBTIZNOST"."ID")))
  WHERE ("STAV"."ID" = 2);


ALTER TABLE public."VITEZOVE" OWNER TO postgres;

--
-- TOC entry 2761 (class 2604 OID 16494)
-- Name: HRA ID; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."HRA" ALTER COLUMN "ID" SET DEFAULT nextval('public."HRA_ID_seq"'::regclass);


--
-- TOC entry 2757 (class 2604 OID 16433)
-- Name: MINA ID; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."MINA" ALTER COLUMN "ID" SET DEFAULT nextval('public."MINA_ID_seq"'::regclass);


--
-- TOC entry 2760 (class 2604 OID 16478)
-- Name: OBLAST ID; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."OBLAST" ALTER COLUMN "ID" SET DEFAULT nextval('public."OBLAST_ID_seq"'::regclass);


--
-- TOC entry 2754 (class 2604 OID 16403)
-- Name: OBTIZNOST ID; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."OBTIZNOST" ALTER COLUMN "ID" SET DEFAULT nextval('public."OBTIZNOST_ID_seq"'::regclass);


--
-- TOC entry 2758 (class 2604 OID 16446)
-- Name: OMEZENI ID; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."OMEZENI" ALTER COLUMN "ID" SET DEFAULT nextval('public."OMEZENI_ID_seq"'::regclass);


--
-- TOC entry 2756 (class 2604 OID 16425)
-- Name: POLE ID; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."POLE" ALTER COLUMN "ID" SET DEFAULT nextval('public."POLE_ID_seq"'::regclass);


--
-- TOC entry 2755 (class 2604 OID 16414)
-- Name: STAV ID; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."STAV" ALTER COLUMN "ID" SET DEFAULT nextval('public."STAV_ID_seq"'::regclass);


--
-- TOC entry 2759 (class 2604 OID 16454)
-- Name: TAH ID; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."TAH" ALTER COLUMN "ID" SET DEFAULT nextval('public."TAH_ID_seq"'::regclass);


--
-- TOC entry 2777 (class 2606 OID 16496)
-- Name: HRA HRA_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."HRA"
    ADD CONSTRAINT "HRA_pkey" PRIMARY KEY ("ID");


--
-- TOC entry 2769 (class 2606 OID 16435)
-- Name: MINA MINA_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."MINA"
    ADD CONSTRAINT "MINA_pkey" PRIMARY KEY ("ID");


--
-- TOC entry 2775 (class 2606 OID 16483)
-- Name: OBLAST OBLAST_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."OBLAST"
    ADD CONSTRAINT "OBLAST_pkey" PRIMARY KEY ("ID");


--
-- TOC entry 2763 (class 2606 OID 16408)
-- Name: OBTIZNOST OBTIZNOST_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."OBTIZNOST"
    ADD CONSTRAINT "OBTIZNOST_pkey" PRIMARY KEY ("ID");


--
-- TOC entry 2771 (class 2606 OID 16448)
-- Name: OMEZENI OMEZENI_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."OMEZENI"
    ADD CONSTRAINT "OMEZENI_pkey" PRIMARY KEY ("ID");


--
-- TOC entry 2767 (class 2606 OID 16427)
-- Name: POLE POLE_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."POLE"
    ADD CONSTRAINT "POLE_pkey" PRIMARY KEY ("ID");


--
-- TOC entry 2765 (class 2606 OID 16419)
-- Name: STAV STAV_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."STAV"
    ADD CONSTRAINT "STAV_pkey" PRIMARY KEY ("ID");


--
-- TOC entry 2773 (class 2606 OID 33023)
-- Name: TAH TAH_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."TAH"
    ADD CONSTRAINT "TAH_pkey" PRIMARY KEY ("ID");


--
-- TOC entry 2786 (class 2620 OID 33012)
-- Name: OBLAST TGR_NEW_GAME; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER "TGR_NEW_GAME" AFTER INSERT ON public."OBLAST" FOR EACH ROW EXECUTE PROCEDURE public."TG_NEW_GAME"();


--
-- TOC entry 2784 (class 2620 OID 32976)
-- Name: OBTIZNOST TGR_OBTIZNOST; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER "TGR_OBTIZNOST" BEFORE INSERT ON public."OBTIZNOST" FOR EACH ROW EXECUTE PROCEDURE public."TG_OBTIZNOST"();


--
-- TOC entry 2785 (class 2620 OID 33032)
-- Name: TAH TGR_TAH; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER "TGR_TAH" BEFORE INSERT ON public."TAH" FOR EACH ROW EXECUTE PROCEDURE public."TG_TAH"();


--
-- TOC entry 2783 (class 2606 OID 16528)
-- Name: HRA HRA_OBLAST_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."HRA"
    ADD CONSTRAINT "HRA_OBLAST_fkey" FOREIGN KEY ("OBLAST") REFERENCES public."OBLAST"("ID");


--
-- TOC entry 2782 (class 2606 OID 16497)
-- Name: HRA HRA_STAV_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."HRA"
    ADD CONSTRAINT "HRA_STAV_fkey" FOREIGN KEY ("STAV") REFERENCES public."STAV"("ID");


--
-- TOC entry 2779 (class 2606 OID 16436)
-- Name: MINA MINA_POLE_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."MINA"
    ADD CONSTRAINT "MINA_POLE_fkey" FOREIGN KEY ("POLE") REFERENCES public."POLE"("ID");


--
-- TOC entry 2781 (class 2606 OID 16484)
-- Name: OBLAST OBLAST_OBTIZNOST_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."OBLAST"
    ADD CONSTRAINT "OBLAST_OBTIZNOST_fkey" FOREIGN KEY ("OBTIZNOST") REFERENCES public."OBTIZNOST"("ID");


--
-- TOC entry 2778 (class 2606 OID 16512)
-- Name: POLE POLE_OBLAST_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."POLE"
    ADD CONSTRAINT "POLE_OBLAST_fkey" FOREIGN KEY ("OBLAST") REFERENCES public."OBLAST"("ID");


--
-- TOC entry 2780 (class 2606 OID 16541)
-- Name: TAH TAH_POLE_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."TAH"
    ADD CONSTRAINT "TAH_POLE_fkey" FOREIGN KEY ("POLE") REFERENCES public."POLE"("ID");


-- Completed on 2018-06-08 10:46:43

--
-- PostgreSQL database dump complete
--

