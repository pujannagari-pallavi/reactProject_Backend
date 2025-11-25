-- Make Vendor LogoUrl and GalleryImages nullable
ALTER TABLE Vendors ALTER COLUMN LogoUrl NVARCHAR(500) NULL;
ALTER TABLE Vendors ALTER COLUMN GalleryImages NVARCHAR(MAX) NULL;
