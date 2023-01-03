Refactoring in C#

Description
You are asked to refactor the UserService class and more
specifically its AddUser method. Assume that the code in
sound in terms of business logic and only focus on applying
clean code principles. Keep in mind acronyms such as SOLID,
KISS, DRY and YAGNI.

Limitations
The Program.cs in the LegacyApp.Consumer shall NOT CHANGE AT ALL.
This includes using statements. Assume that this codebase is part of
a greater system and any non-backwards compatibile change will break the system.
Any sort of change in that class will result in you instantly failing
this test.

You can change anything in the LegacyApp project except for the UserDataAccess
class and its AddUser method. Both NEED to stay static.