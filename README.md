Use this Script

CREATE TABLE "UserMaster" (
    "UserId" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "Email" VARCHAR(255) UNIQUE NOT NULL,
    "Password" TEXT NOT NULL,
    "Role" VARCHAR(50) NOT NULL,
    "IsActive" BOOLEAN DEFAULT TRUE,
    "CreatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);


INSERT INTO "UserMaster" ("Email", "Password", "Role")
VALUES
('superadmin@gmail.com', 'ef6129290c7d371da2598cd4c1e9725514397d18ebabca118c206df1ec6c5a11', 'SuperAdmin'),
('admin@gmail.com', 'a36aef5a11c4073fbe60314fc9df530a9d5f986533594d1f5190742ff9e0e408', 'Admin'),
('manager@gmail.com', 'f3cdf766bd8256fded84ed1590ab9bbc192ee1e930bd86b450905dfa09d31105', 'Manager'),
('employee@gmail.com', '22583d42e0a169663eeed2d0f6e090bcf3ee83815c39c9334a75b026ef2e844a', 'Employee');


CREATE TABLE "PropertiesMaster" (
    "PropertyTypeId" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "PropertyType" VARCHAR(50) NOT NULL UNIQUE,
    "IsActive" BOOLEAN DEFAULT TRUE,
    "CreatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

INSERT INTO "PropertiesMaster" ("PropertyType")
VALUES
('Apartment'),
('Villa'),
('Land'),
('Individual House'),
('Commercial Space');


CREATE TABLE "PropertiesDetailsMaster" (
    "PropertiesDetailsId" UUID PRIMARY KEY DEFAULT gen_random_uuid(),

    "PropertyTitle" VARCHAR(255) NOT NULL,
    "Price" NUMERIC(18,2) NOT NULL,
    "PropertyType" VARCHAR(50) NOT NULL, -- Apartment, Villa, Plot, etc.
	"PropertySubType" VARCHAR(50) NOT NULL,
    "IsLoanProviding" BOOLEAN DEFAULT FALSE,

    "PropertySqFt" NUMERIC(18,2),
    "PlotAreaSqYd" NUMERIC(18,2),
    "Bedrooms" INT,
    "Bathrooms" INT,
    "NumberOfFloors" INT,
    "FloorNumber" INT,
    "MonthlyMaintenance" NUMERIC(18,2),
    "Washrooms" INT,
    "CommercialType" VARCHAR(100),
    "IsGovApproved" BOOLEAN,

    "FurnishingStatus" VARCHAR(50), -- Unfurnished, Semi-Furnished, Fully-Furnished
    "FacingDirection" VARCHAR(20),  -- East, West, North, South
    "AgeOfProperty" INT,

    "HasSwimmingPool" BOOLEAN DEFAULT FALSE,
    "HasGym" BOOLEAN DEFAULT FALSE,
    "HasSecurity" BOOLEAN DEFAULT FALSE,
    "HasParking" BOOLEAN DEFAULT FALSE,
    "HasClubHouse" BOOLEAN DEFAULT FALSE,
    "HasPowerBackup" BOOLEAN DEFAULT FALSE,
    "HasLift" BOOLEAN DEFAULT FALSE,
    "HasGarden" BOOLEAN DEFAULT FALSE,
    "HasKidsPlayArea" BOOLEAN DEFAULT FALSE,
    "HasCCTV" BOOLEAN DEFAULT FALSE,
    "HasIntercom" BOOLEAN DEFAULT FALSE,
    "HasFireSafety" BOOLEAN DEFAULT FALSE,
    "HasWaterSupply24x7" BOOLEAN DEFAULT FALSE,

    "ImageUrls" TEXT,
    "VideoUrl1" TEXT,
    "VideoUrl2" TEXT,

    "Description" TEXT,
    "Location" TEXT,
	"LocationIframe" TEXT,

    "CreatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    "CreatedBy" UUID,
	
    "UpdatedAt" TIMESTAMP,
    "UpdatedBy" UUID,
    "IsActive" BOOLEAN DEFAULT TRUE
);


CREATE TABLE "CustomerContactMaster" (
    "ContactId" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "FullName" VARCHAR(150) NOT NULL,
    "Email" VARCHAR(255) NOT NULL,
    "PhoneNumber" VARCHAR(20),
    "CustomerInterest" VARCHAR(150),
    "Message" TEXT,
    "PropertyId" UUID, 
    "SubmittedDate" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    "IsActive" BOOLEAN DEFAULT TRUE
);

select * from "UserMaster"
select * from "PropertiesMaster"
select * from "PropertiesDetailsMaster"

ALTER TABLE "PropertiesDetailsMaster"
ALTER COLUMN "ImageUrls" TYPE TEXT
