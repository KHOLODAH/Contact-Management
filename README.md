# Contact-Management

This is a simple Console-based Contact Management System built in C# that allows you to add, edit, delete, view, search, and filter contacts. All data is stored in a JSON file (contacts.json).

Features:

Add a new contact with Name, Phone, and Email.

Edit an existing contact.

Delete a contact by ID.

View a contact by ID.

List all contacts.

Search contacts by Name or Phone.

Filter contacts by:

Creation date

First name

How to Run
-Use the menu to manage contacts:
=== Contact Management System ===
1. Add Contact
2. Edit Contact
3. Delete Contact
4. View Contact
5. List All Contacts
6. Search
7. Filter
8. Save Contacts
9. Exit
Enter your choice:


-Contacts are automatically loaded from contacts.json on startup.
Any changes can be saved manually using option 8, or automatically when exiting (option 9).



-Notes

Contacts are stored in a JSON file (contacts.json) in the same directory.

Each contact has a unique ID to prevent duplicates.

When editing or deleting, you need to enter the contact ID.

Dates must be entered in the format yyyy-MM-dd when filtering.
