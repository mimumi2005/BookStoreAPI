
# Book store API

## Running the program

1. Download the repository as zip;
2. Uzip the folder;
3. Run the folder with visual studio code or other IDE;
4. Open a terminal and type ```dotnet run```;

## Testing the API

1. Open any web browser and type ```localhost:5000```;
2. The Swagger UI will be displayed and the CRUD functions will be shown with descriptions;
3. First create database information by using the POST route and creating multiple books for testing, to create a book, ur required to send a JSON file with all the neccesary information;
4. Then you can test the GET functions which theres the main ```/api/Books``` which has pagination and has an option to sort by authors, or the ```/api/Books/{id}```, which gets a specific book by id;
5. Lastly you can test the PUT (editing) and DELETE (removing) routes, where the PUT one requires the ID and book data in JSON, while the DELETE one only requires the ID of the book to delete

## 🛠 Skills
Controllers, C#
