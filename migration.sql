CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;

ALTER DATABASE CHARACTER SET utf8mb4;

CREATE TABLE `AspNetRoles` (
    `Id` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Name` varchar(256) CHARACTER SET utf8mb4 NULL,
    `NormalizedName` varchar(256) CHARACTER SET utf8mb4 NULL,
    `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_AspNetRoles` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `AspNetUsers` (
    `Id` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `UserName` varchar(256) CHARACTER SET utf8mb4 NULL,
    `NormalizedUserName` varchar(256) CHARACTER SET utf8mb4 NULL,
    `Email` varchar(256) CHARACTER SET utf8mb4 NULL,
    `NormalizedEmail` varchar(256) CHARACTER SET utf8mb4 NULL,
    `EmailConfirmed` tinyint(1) NOT NULL,
    `PasswordHash` longtext CHARACTER SET utf8mb4 NULL,
    `SecurityStamp` longtext CHARACTER SET utf8mb4 NULL,
    `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 NULL,
    `PhoneNumber` longtext CHARACTER SET utf8mb4 NULL,
    `PhoneNumberConfirmed` tinyint(1) NOT NULL,
    `TwoFactorEnabled` tinyint(1) NOT NULL,
    `LockoutEnd` datetime(6) NULL,
    `LockoutEnabled` tinyint(1) NOT NULL,
    `AccessFailedCount` int NOT NULL,
    CONSTRAINT `PK_AspNetUsers` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Categories` (
    `CategoryId` int NOT NULL AUTO_INCREMENT,
    `CategoryName` longtext CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Categories` PRIMARY KEY (`CategoryId`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Products` (
    `id` int NOT NULL AUTO_INCREMENT,
    `Title` longtext CHARACTER SET utf8mb4 NOT NULL,
    `OnlyInNovus` longtext CHARACTER SET utf8mb4 NULL,
    `Brand` longtext CHARACTER SET utf8mb4 NULL,
    `Category` longtext CHARACTER SET utf8mb4 NULL,
    `Subcategory` longtext CHARACTER SET utf8mb4 NULL,
    `Promo` longtext CHARACTER SET utf8mb4 NULL,
    `Country` longtext CHARACTER SET utf8mb4 NULL,
    `Method` longtext CHARACTER SET utf8mb4 NULL,
    `Basis` longtext CHARACTER SET utf8mb4 NULL,
    `Temperature` longtext CHARACTER SET utf8mb4 NULL,
    `Composition` longtext CHARACTER SET utf8mb4 NULL,
    `Description` longtext CHARACTER SET utf8mb4 NULL,
    `Expiration_date` longtext CHARACTER SET utf8mb4 NULL,
    `Storage_conditions` longtext CHARACTER SET utf8mb4 NULL,
    `Caloric` longtext CHARACTER SET utf8mb4 NULL,
    `Carbohydrate` longtext CHARACTER SET utf8mb4 NULL,
    `Fat` longtext CHARACTER SET utf8mb4 NULL,
    `Protein` longtext CHARACTER SET utf8mb4 NULL,
    `Allergen` longtext CHARACTER SET utf8mb4 NULL,
    `Refuel` longtext CHARACTER SET utf8mb4 NULL,
    `Quantity_in_package` longtext CHARACTER SET utf8mb4 NULL,
    `Energy_value` longtext CHARACTER SET utf8mb4 NULL,
    `Sort` longtext CHARACTER SET utf8mb4 NULL,
    `Features` longtext CHARACTER SET utf8mb4 NULL,
    `For_children_with` longtext CHARACTER SET utf8mb4 NULL,
    `Microelements` longtext CHARACTER SET utf8mb4 NULL,
    `Vitamins` longtext CHARACTER SET utf8mb4 NULL,
    `Type_of_cheese` longtext CHARACTER SET utf8mb4 NULL,
    `Type_of_sausage` longtext CHARACTER SET utf8mb4 NULL,
    `Method_of_processing` longtext CHARACTER SET utf8mb4 NULL,
    `By_composition` longtext CHARACTER SET utf8mb4 NULL,
    `Quantity_box_package` longtext CHARACTER SET utf8mb4 NULL,
    `Diaper_size` longtext CHARACTER SET utf8mb4 NULL,
    `Alcohol` longtext CHARACTER SET utf8mb4 NULL,
    `Temperature_wine_serving` longtext CHARACTER SET utf8mb4 NULL,
    `Region` longtext CHARACTER SET utf8mb4 NULL,
    `Wine_classification` longtext CHARACTER SET utf8mb4 NULL,
    `Aging_in_barrel` longtext CHARACTER SET utf8mb4 NULL,
    `Package_volume` longtext CHARACTER SET utf8mb4 NULL,
    `Price` decimal(65,30) NULL,
    CONSTRAINT `PK_Products` PRIMARY KEY (`id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `SliderImages` (
    `Id` int NOT NULL AUTO_INCREMENT,
    CONSTRAINT `PK_SliderImages` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Subcategories` (
    `SubcategoryId` int NOT NULL AUTO_INCREMENT,
    `CategoryId` int NOT NULL,
    `SubcategoryName` longtext CHARACTER SET utf8mb4 NOT NULL,
    `CountOfProducts` int NOT NULL,
    CONSTRAINT `PK_Subcategories` PRIMARY KEY (`SubcategoryId`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `AspNetRoleClaims` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `RoleId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `ClaimType` longtext CHARACTER SET utf8mb4 NULL,
    `ClaimValue` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_AspNetRoleClaims` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_AspNetRoleClaims_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `AspNetUserClaims` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `UserId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `ClaimType` longtext CHARACTER SET utf8mb4 NULL,
    `ClaimValue` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_AspNetUserClaims` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_AspNetUserClaims_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `AspNetUserLogins` (
    `LoginProvider` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `ProviderKey` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `ProviderDisplayName` longtext CHARACTER SET utf8mb4 NULL,
    `UserId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_AspNetUserLogins` PRIMARY KEY (`LoginProvider`, `ProviderKey`),
    CONSTRAINT `FK_AspNetUserLogins_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `AspNetUserRoles` (
    `UserId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `RoleId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_AspNetUserRoles` PRIMARY KEY (`UserId`, `RoleId`),
    CONSTRAINT `FK_AspNetUserRoles_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_AspNetUserRoles_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `AspNetUserTokens` (
    `UserId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `LoginProvider` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Name` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Value` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_AspNetUserTokens` PRIMARY KEY (`UserId`, `LoginProvider`, `Name`),
    CONSTRAINT `FK_AspNetUserTokens_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE INDEX `IX_AspNetRoleClaims_RoleId` ON `AspNetRoleClaims` (`RoleId`);

CREATE UNIQUE INDEX `RoleNameIndex` ON `AspNetRoles` (`NormalizedName`);

CREATE INDEX `IX_AspNetUserClaims_UserId` ON `AspNetUserClaims` (`UserId`);

CREATE INDEX `IX_AspNetUserLogins_UserId` ON `AspNetUserLogins` (`UserId`);

CREATE INDEX `IX_AspNetUserRoles_RoleId` ON `AspNetUserRoles` (`RoleId`);

CREATE INDEX `EmailIndex` ON `AspNetUsers` (`NormalizedEmail`);

CREATE UNIQUE INDEX `UserNameIndex` ON `AspNetUsers` (`NormalizedUserName`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20240102105417_Identity', '7.0.14');

COMMIT;

