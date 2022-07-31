﻿CREATE TABLE Weather (
    Id INT IDENTITY(1,1) PRIMARY KEY, 
    [Date] DATETIME2 NOT NULL, 
    TemperatureC INT NOT NULL, 
    Summary VARCHAR(100) NOT NULL
)