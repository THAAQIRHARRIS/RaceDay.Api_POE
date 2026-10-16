# RaceDay.Api
PROG PART2 POE
Part 2: RESTful API Development

Part 2 turns the Part 1 plan into a working ASP.NET Core 8 Web API for RaceDay. It uses Entity Framework Core (code-first) with SQL Server, and the database matches the Part 1 ERD and SQL script.

Authentication and roles

Users register as an Organiser or a Participant. Organiser registration needs a secret organiser code, so nobody can pick that role for themselves.
Passwords are hashed with BCrypt and never stored in plain text.
Login creates a server-side session that stores the user’s id and role.
A custom [SessionAuthorize] filter returns 401 when the user isn’t logged in and 403 when the role is wrong. Organisers can only manage their own events.

Endpoints

Authentication and profile: register, login, logout, and view or update your own profile.
Events: Organisers create, update and delete events, and everyone can view them.
Categories: Organisers manage categories per event, and everyone can view them.
Enrolments: Participants enrol in an event under a category, view their own enrolments, and cancel. Organisers view all enrolments for their events and update their status.
Results: Organisers capture and correct finish times and positions, Participants view their own results, and each event has a public results list.

Swagger

Every endpoint appears at /swagger with a description, the role required and its response codes. You can test them in the browser after logging in.

Unit testing

An xUnit test project starts the real application against an in-memory database and tests the Program.cs wiring: Swagger, dependency injection, the seeded event types, session login and logout, and role enforcement.

Deviations from the Part 1 plan

I added organiserCode to registration.
I added GET /api/events/mine.
I added PUT /api/enrolments/{id}/status.
Event listing returns upcoming events only by default.
Cancelling an enrolment deletes it.
The banner and profile-image uploads are left for Part 3, because they depend on Azure Blob Storage.

YouTube vid:
