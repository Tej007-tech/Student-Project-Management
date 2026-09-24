-- ============================================================================
-- SPMS SQL Server Database Seed Script
-- Populates SPM_ProjectMaster and SPM_ProjectAllocation tables with realistic data.
-- ============================================================================

USE [StudentProjectDB]; -- Adjust database name if needed
GO

-- 1. Insert Project Masters if not already present
IF NOT EXISTS (SELECT 1 FROM [dbo].[SPM_ProjectMaster] WHERE [ProjectID] = 1)
BEGIN
    SET IDENTITY_INSERT [dbo].[SPM_ProjectMaster] ON;

    INSERT INTO [dbo].[SPM_ProjectMaster] ([ProjectID], [ProjectTitle], [Description])
    VALUES 
    (1, N'Smart Parking System using IoT', N'An automated parking management system using IoT sensors, real-time slot tracking, and mobile app reservation.'),
    (2, N'AI Crop Disease Detection', N'Deep learning computer vision app to identify plant diseases from leaf imagery and recommend organic treatments.'),
    (3, N'Campus Event Portal & Ticket System', N'A centralized platform for university club events, digital QR ticketing, and automated attendance logging.'),
    (4, N'Blockchain Certificate Verification', N'Decentralized ledger application for issuing and tamper-proof verification of academic credentials.');

    SET IDENTITY_INSERT [dbo].[SPM_ProjectMaster] OFF;
END
GO

-- 2. Insert Project Allocations mapped to existing Students and Faculty
-- Assumes Student UserID = 3 (Alex Johnson), Faculty UserID = 2 (Dr. Robert Vance)
IF NOT EXISTS (SELECT 1 FROM [dbo].[SPM_ProjectAllocation] WHERE [ProjectAllocationID] = 1)
BEGIN
    SET IDENTITY_INSERT [dbo].[SPM_ProjectAllocation] ON;

    INSERT INTO [dbo].[SPM_ProjectAllocation] 
    ([ProjectAllocationID], [ProjectID], [StudentID], [FacultyID], [AssignedDate], [ProjectStartDate], [ProjectEndDate], [TotalTasksGiven], [TotalCompletedTasks], [ProgressPercentage], [OverAllGrade])
    VALUES 
    (1, 1, 3, 2, '2026-01-10', '2026-01-15', '2026-05-30', 5, 3, 60.00, N'A'),
    (2, 2, 3, 2, '2026-02-01', '2026-02-05', '2026-06-15', 4, 1, 25.00, NULL),
    (3, 3, 4, 2, '2026-01-20', '2026-02-01', '2026-05-20', 6, 6, 100.00, N'A+');

    SET IDENTITY_INSERT [dbo].[SPM_ProjectAllocation] OFF;
END
GO
