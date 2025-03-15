# TVMaze Scraper

## Background
The TVMaze Scraper application scrapes show and cast information from the TVMaze API, stores the data, and provides it through a custom REST API. The TVMaze database provides public data through a rate-limited API: [TVMaze API](http://www.tvmaze.com/api).

## Assignment
This application:
1. Scrapes the TVMaze API for TV show and cast data.
2. Persists the data in storage (SQLite).
3. Exposes a REST API to provide the scraped data.

## Requirements
The REST API should:
- Provide a paginated list of TV shows, with each show’s ID and cast.
- Cast members should be ordered by birthday in descending order.

### Example Response
```json
[
  {
    "id": 1,
    "name": "Game of Thrones",
    "cast": [
      { "id": 7, "name": "Mike Vogel", "birthday": "1979-07-17" },
      { "id": 9, "name": "Dean Norris", "birthday": "1963-04-08" }
    ]
  },
  {
    "id": 4,
    "name": "Big Bang Theory",
    "cast": [
      { "id": 6, "name": "Michael Emerson", "birthday": "1950-01-01" }
    ]
  }
]
```
## Setup
### 1. Clone the Repository
```bash
git clone <repository_url>
```
### 2. Navigate to the Project Directory
```bash
cd <project_directory>
```
### 3. Install Dependencies
```bash
dotnet restore
```
### 4. Apply Migrations to Set Up the Database
```bash
dotnet ef database update
```
### 5. Run the Application
```bash
dotnet run
```

## Guidelines
- Implemented using the latest version of .NET (.NET 9).
- Made the solution available in a public Git repository during the review process.
- Cover the behavior with tests. (TODO)



