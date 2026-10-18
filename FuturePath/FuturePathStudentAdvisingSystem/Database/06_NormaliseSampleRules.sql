-- Convert only the explicit sample alternative rows; no guessing from programme names.
USE FuturePathAdvisingSystem;
GO
SET XACT_ABORT ON;
BEGIN TRANSACTION;
UPDATE dbo.ProgrammeRequirement SET AlternativeGroup='Sample numeracy choice'
WHERE AlternativeGroup IS NULL AND IsMandatory=0 AND AdvisoryNote LIKE 'Sample alternative:%';
-- Do not edit or replace stored historic recommendation scores. Recalculate in a new/current session.
COMMIT;
GO
