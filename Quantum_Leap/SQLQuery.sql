
Select * from Leapees;

Select * from Events

Select * from Leap

Select * from Leapers


-- Insert Leap
declare @leaperId int = 1
declare @eventId int =1
declare @leapeeId int =1
declare @date datetime = '1957-11-09'
declare @cost decimal = 13
Insert into Leap (leaperId, leapeeId, eventId, date, cost)
Output inserted.*
Values(@leaperId, @leapeeId, @eventId, @date, @cost);

--Get Leap with leaper,leapee and Event information
Select l.Id, l.Cost as LeapCost, lr.LeaperName, le.LeapeeName, e.EventName, e.Description, e.Date, e.Location
From leap as l
Join Leapers as lr
On l.LeaperId = lr.Id
Join Leapees as le
On l.LeapeeId = le.Id
Join Events as e
On l.EventId = e.Id;

--Random Leaper
SELECT TOP(1) lr.* FROM Leapers as lr
ORDER BY NEWID()

--Random Event
Select TOP(1) e.* 
From Events as e
Where e.isCorrected = 0
Order By NEWID()

--Random Leapee
Select TOP(1) le.* 
From Leapees as le
Order By NEWID()

--update Leap
Update Leap 
Set Cost = 35000,	
	LeaperId = 2,
	LeapeeId = 2,
	EventId = 3,
	Date = '1954-09-04'
Where id = 3;

--getting events based on leapee
Select le.*, e. * 
from Leapees as le, events as e 
Where le.Id = e.LeapeeId;

--getting random leapee with Event information
Select TOP(1) le.LeapeeName, e.EventName, e.Description, e.Date,e.Location, e.IsCorrected 
From events as e, Leapees as le
Where e.LeapeeId = le.Id
Order By NEWID();

Select * from Leapees;
Select TOP(1) Leapees.Id from Leapees where Id in (Select leapeeId from events) 
Order By NEWID();


-- updating leaper with budgeted amount
declare @cost1 decimal(8,2) = 10000
declare @leaperId1 int = 1
Update Leapers Set BudgetAmount = BudgetAmount - @cost1 Where Id = @leaperId1

Select * from Leap

-- query for ensuring Leap table doesn't have existing event and getting event
declare @leapeeId2 int = 1
Select e.Id as EventId
From Events as e
Where e.LeapeeId = @leapeeId2 and e.IsCorrected = 0 
and e.Id Not In(Select EventId from Leap)

