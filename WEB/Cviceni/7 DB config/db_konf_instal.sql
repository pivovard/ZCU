-- MySQL Script generated by MySQL Workbench
-- 11/03/15 10:46:56
-- Model: New Model    Version: 1.0
-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Table `nyklm_prava`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `nyklm_prava` ;

CREATE TABLE IF NOT EXISTS `nyklm_prava` (
  `idprava` INT NOT NULL,
  `nazev` VARCHAR(20) NOT NULL,
  `vaha` INT(2) NOT NULL,
  PRIMARY KEY (`idprava`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `nyklm_uzivatele`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `nyklm_uzivatele` ;

CREATE TABLE IF NOT EXISTS `nyklm_uzivatele` (
  `iduzivatel` INT NOT NULL AUTO_INCREMENT,
  `jmeno` VARCHAR(60) NOT NULL,
  `login` VARCHAR(30) NOT NULL,
  `heslo` VARCHAR(40) NOT NULL,
  `email` VARCHAR(35) NOT NULL,
  `idprava` INT NOT NULL DEFAULT 3,
  PRIMARY KEY (`iduzivatel`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;

-- -----------------------------------------------------
-- Data for table `nyklm_prava`
-- -----------------------------------------------------
START TRANSACTION;
INSERT INTO `nyklm_prava` (`idprava`, `nazev`, `vaha`) VALUES (1, 'Administrator', 10);
INSERT INTO `nyklm_prava` (`idprava`, `nazev`, `vaha`) VALUES (2, 'Recenzent', 5);
INSERT INTO `nyklm_prava` (`idprava`, `nazev`, `vaha`) VALUES (3, 'Autor', 2);

COMMIT;


-- -----------------------------------------------------
-- Data for table `nyklm_uzivatele`
-- -----------------------------------------------------
START TRANSACTION;
INSERT INTO `nyklm_uzivatele` (`iduzivatel`, `jmeno`, `login`, `heslo`, `email`, `idprava`) VALUES (1, 'Pokusný administrátor', 'admin', 'admin', 'pokus1@kiv.zcu.cz', 1);
INSERT INTO `nyklm_uzivatele` (`iduzivatel`, `jmeno`, `login`, `heslo`, `email`, `idprava`) VALUES (2, 'Pokusný recenzent', 'pokus', 'pokus', 'pokus2@kiv.zcu.cz', 2);

COMMIT;

