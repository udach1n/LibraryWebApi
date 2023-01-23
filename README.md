# LibraryWebApi

My Task:

The Library has users and books. There are 2 types of users admin and user. The user can take books, but take no ore than 3 books.

The user can take books from the Library and the adnin can delete, add to the library, receive a report which books are more than 1 month in the hands of the user.

You can check the work of this task through postman or swagger.

There is a json File(Library.postman_collection.json), it belongs to the postman and it says what requests are responsible for what.



https://localhost:44377/Anonymous/AllBooks - Show all books
https://localhost:44377/Anonymous/BookByTitle?title=i - Show all books are containing "i"
https://localhost:44377/Account/Register - Register new user
https://locathost:44377/Account/Login - Login user
https://Localhost:44377/Account/Logout - Logout

this is for logged in users with role "user"
https://localhost:44377/User/User`sBooks - All books of the currently logged in user
https://localhost:44377/User/AddBookList?idbook=4 - Add book with idBook 4 to user's list
https://localhost:44377/User/User`sBook?idBook=4 - does the user have a book with idBook 4
https://localhost:44377/User/DelBookList?delBook=2 - Delete book with idBook 2 in user's list

this is for logged in users with role "admin"
adminemail = "admin@gmail.com";
adminpassword = "123456";
https://localhost:44377/Admin/InfoUser?iduser=2 - information about the user's with iduser 2 books that he has in his hands for more than 1 month
https://localhost:44377/Admin/AddBookLibrary - Add book to Library by admin
https://localhost:44377/Admin/DelBookLibrary?idBook=19 - Delete book with IdBook 19 in Library by admin


