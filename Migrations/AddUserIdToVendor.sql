-- Add UserId column to Vendors table and create foreign key relationship
ALTER TABLE Vendors ADD UserId INT NULL;

-- Add foreign key constraint with cascade delete
ALTER TABLE Vendors ADD CONSTRAINT FK_Vendors_Users_UserId 
FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE;

-- Update existing vendors to link with their user accounts (match by email)
UPDATE v
SET v.UserId = u.Id
FROM Vendors v
INNER JOIN Users u ON v.Email = u.Email
WHERE u.Role = 3; -- Vendor role

-- Create index for better query performance
CREATE INDEX IX_Vendors_UserId ON Vendors(UserId);
