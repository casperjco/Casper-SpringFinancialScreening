# Casper-SpringFinancialScreening

To setup the MySQL database, run the following command:
docker-compose up -d

The database setup SQL can be found in /Database/DatabaseSchema.txt

Hit the /api/product/generate endpoint to seed the database with 1000 randomly generated products.

Given more time to flesh out the project, I would have added additional unit tests, proper logging for analytics and metrics, edge-case and error-case handling.

I would have cleaned up the frontend and gave it some better styling, improved the display of the products, add security and authentication, and added some user friendliness like alternate text, hover-over text or help text.
Jest could be setup to do some UI testing with snapshots as well.

I also would have setup dependency injection properly and setup a/b (green/blue) testing capabilities, with a properly hosted website that can be scaled up, possibly multi-region fan-out for a global audience if that were a consideration.

Caching would definitely be setup, no need to make database connections for every query when some of the more popular queries would likely always be the same.
Pagination would be a must, as the product database grows it is both resource-intensive and not necessary to pull all products in one request.
