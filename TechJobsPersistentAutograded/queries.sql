--Part 1
SELECT COLUMN_NAME, DATA_TYPE  FROM INFORMATION_SCHEMA.COLUMNS 
  WHERE table_name = 'jobs'

--Part 2
SELECT * FROM techjobs.employers
WHERE Location= "St. Louis";

--Part 3
