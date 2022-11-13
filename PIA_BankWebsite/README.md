# KIV/PIA Bank

This project is semestral work of KIV/PIA.
Web application represents the internet banking of a fictional bank.

## Technologies

Application is created in ASP.NET Core MVC and Entity framework core.
Database is realized as Microsoft SQL Server.
Tests are written as MSTest unit tests.

## Deployment

Both application and database are deployed on Microsoft Azure servers:
https://bank20190117021846.azurewebsites.net

### Users

Admin accounts:
- Admin001    1234
- 147         147

User accounts (set valid email for verification):
- User0001    0001
- User0002    0002
- 123         123
- 27141478    2891

## Roles

### Not signed
Can view only public (static) pages.

### Admin
Can manage users (add, edit, delete).
Can't make bank operations.

### User
Can manage his credentials (not account information).
Can send payment. Can use template for payment.
Can add/edit/delete template for payment.

## Optional functions
Email notice when account is created/edited/deleted.
Email login verification.
Email payment confirmation.
Confirmation of paymen if destination account within same bank code doesn't exist.
Transfer money within same bank.

