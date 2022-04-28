
delete from Surveys;
delete from Questions;
delete from Options;
delete from Participants;
delete from Selections;

DBCC CHECKIDENT ('Surveys', RESEED, 0);
DBCC CHECKIDENT ('Questions', RESEED, 0);
DBCC CHECKIDENT ('Options', RESEED, 0);
DBCC CHECKIDENT ('Participants', RESEED, 0);
DBCC CHECKIDENT ('Selections', RESEED, 0);

