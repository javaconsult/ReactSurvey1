/****** Script for SelectTopNRows command from SSMS  ******/
delete from Surveys;
delete from Questions;
delete from Options;
delete from Participants;
delete from Selections;

DBCC CHECKIDENT ('Questions', RESEED, 0);
DBCC CHECKIDENT ('Options', RESEED, 0);
DBCC CHECKIDENT ('Selections', RESEED, 0);


CREATE USER dineenwebapp1 FROM EXTERNAL PROVIDER
ALTER ROLE db_datareader ADD MEMBER dineenwebapp1
ALTER ROLE db_datawriter ADD MEMBER dineenwebapp1

select * from AspNetUsers;

select * from Surveys;
select * from Questions;
select * from Options;

select * from Participants;
select * from Selections;

select questions.text,  options.Text, QuestionId, Options.Id as OptionId from questions inner join options on questions.id = options.questionid

