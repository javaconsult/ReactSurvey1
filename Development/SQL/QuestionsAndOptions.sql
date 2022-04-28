
select questions.text as Question, options.Text as [Option], QuestionId, Options.Id as OptionId 
from questions 
inner join options on questions.id = options.questionid
