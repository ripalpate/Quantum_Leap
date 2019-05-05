Select * from Leapees

Select * from Leapers

Select * from Events

Select * from Leap

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
Select l.Id, l.Cost, lr.*, e.*, le.*
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


Select l.Id, l.Cost, lr.Name, lr.BudgetAmount
From leap as l
Join Leapers as lr
On l.LeaperId = lr.Id;
