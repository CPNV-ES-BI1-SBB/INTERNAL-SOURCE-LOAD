-- Table for Train
CREATE TABLE Train (
    TrainID INT AUTO_INCREMENT PRIMARY KEY,
    Group VARCHAR(255) NOT NULL, -- Group / category of the train
    Line VARCHAR(255) DEFAULT NULL -- Line the train operates on
);

-- Table for TrainStation
CREATE TABLE TrainStation (
    StationID INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(255) NOT NULL -- Name of the train station
);

-- Table for Departure
CREATE TABLE Departure (
    DepartureID INT AUTO_INCREMENT PRIMARY KEY,
    DepartureStationID INT NOT NULL, -- FK to TrainStation
    DestinationStationName VARCHAR(255) NOT NULL, -- Destination station
    DepartureTime DATETIME NOT NULL, -- Scheduled departure time
    TrainID INT NOT NULL, -- FK to Train
    Platform VARCHAR(255) NOT NULL, -- Departure platform
    Sector VARCHAR(255) DEFAULT NULL, -- Sector, nullable
    FOREIGN KEY (DepartureStationID) REFERENCES TrainStation(StationID) ON DELETE CASCADE,
    FOREIGN KEY (TrainID) REFERENCES Train(TrainID) ON DELETE CASCADE
);

-- Table for ViaStationNames (Intermediate Stations)
CREATE TABLE ViaStationNames (
    ViaID INT AUTO_INCREMENT PRIMARY KEY,
    DepartureID INT NOT NULL, -- FK to Departure
    StationID INT NOT NULL, -- FK to TrainStation
    FOREIGN KEY (DepartureID) REFERENCES Departure(DepartureID) ON DELETE CASCADE
    FOREIGN KEY (StationID) REFERENCES TrainStation(StationID) ON DELETE CASCADE
);
