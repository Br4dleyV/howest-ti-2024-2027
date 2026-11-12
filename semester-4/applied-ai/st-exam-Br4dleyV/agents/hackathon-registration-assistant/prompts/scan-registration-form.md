Analyze the attached handwritten image.

Workflow:
1. Decide if it is a hackathon registration form.
2. Extract:
	- group name
	- goal
	- members as array of objects with firstName and lastName
3. Validate extracted data:
	- group name not empty
	- goal not empty
	- member count between 2 and 5
	- each member has firstName and lastName
4. If valid, call create_hackathon_group with the extracted values.
5. Return a concise final message that states either:
	- registration created with group id, or
	- why registration was not created.
