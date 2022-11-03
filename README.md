# Demo Retail Manager System

## Disclaimer
This app is built under the guidance of Tim Corey free online course - [TimCo Retail Manager Course](https://www.youtube.com/playlist?list=PLLWMQd6PeGY0bEMxObA6dtYXuJOGfxSPx). And though it was written along with the Author's comments and code examples, the final outcome has some major differences.

## Description
The application provides Point of Sale part of retail management system with authentication, User Management, and Restocking of Inventory. The idea behind is to create a small demo cashier registrator that provides access to the Cashier to Sale page, where all the purchase proccess is happening.\
The system itself consists of three layers:
- Database
- API
- WPF user interface

On the first build and running of the API databases will be prepopulated with some inventory items, and two users:
- Admin (admin@drm.com - !A3fpnzUaeLs8)
- Manager (manager@drm.com - 1apMn(XMkjvy))

In order to app successfully run, following steps must be taken:
1. Under "Solution -> Set Startup Projects" select "Multiple", set DRMApi and DRMDesktopUI to Start
2. Publish DRMData database under the "DRMData" name
3. Run both projects, so the databases are populated

After the user is logged in they are allowed to Sale Page to make a purchase or Inventory Page to restock products if needed. Also the user with admin role access is able to reach Users Administration Page, where they could change any user roles.
