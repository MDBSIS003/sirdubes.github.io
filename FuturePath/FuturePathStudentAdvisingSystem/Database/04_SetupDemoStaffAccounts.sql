-- LOCAL DEVELOPMENT ONLY. Inserts missing demo accounts, never resets or promotes an existing account.
USE FuturePathAdvisingSystem;
GO
SET XACT_ABORT ON;
BEGIN TRANSACTION;
IF NOT EXISTS (SELECT 1 FROM dbo.UserAccount WITH (UPDLOCK,HOLDLOCK) WHERE Username='advisor_test')
 INSERT dbo.UserAccount(Username,[Password],UserRole,FirstName,LastName) VALUES
 ('advisor_test','PBKDF2-SHA256$600000$cbv5IAyuvOS4RbRRoX+rYQ==$9mFVG3wrVV/MX1O0E3dfA5k3RqP/9hlA8iTlRSBfN1I=','Career Advisor','Demo','Advisor');
IF NOT EXISTS (SELECT 1 FROM dbo.UserAccount WITH (UPDLOCK,HOLDLOCK) WHERE Username='admin_test')
 INSERT dbo.UserAccount(Username,[Password],UserRole,FirstName,LastName) VALUES
 ('admin_test','PBKDF2-SHA256$600000$ZuW2Nflw6ByEy75TqKSuOw==$3qV6EMkm/+rgJO1WRuM8jmp8+V5/lCR6KbTJCXKb/6w=','System Administrator','Demo','Administrator');
COMMIT;
GO
