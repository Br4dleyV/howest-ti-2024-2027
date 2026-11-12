/* 
ADD ALL NECESSARY TABLES, SEED DATA and POC DATA 
HINT: start with dropping tables if they exist
See example server for reference
*/
-- Disable foreign key checks temporarily to safely drop tables

SET FOREIGN_KEY_CHECKS = 0;

DROP TABLE IF EXISTS cargo_reservations;
DROP TABLE IF EXISTS reservations;
DROP TABLE IF EXISTS stations;
DROP TABLE IF EXISTS pods;
DROP TABLE IF EXISTS users;

SET FOREIGN_KEY_CHECKS = 1;

-- Create database
CREATE TABLE users(
    id BINARY(16) NOT NULL PRIMARY KEY,
    firstname VARCHAR(255) NOT NULL,
    lastname VARCHAR(255) NOT NULL,
    dob DATE NOT NULL,
    role ENUM('user', 'admin') NOT NULL
);

CREATE TABLE pods(
    id BINARY(16) NOT NULL PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    capacity int not null,
    max_capacity int not null,
    is_active BOOL DEFAULT TRUE,
    current_location POINT NOT NULL
);

CREATE TABLE stations(
    id BINARY(16) NOT NULL PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    coords POINT NOT NULL
);

CREATE TABLE reservations(
    id BINARY(16) NOT NULL PRIMARY KEY,
    user_id BINARY(16) NOT NULL,
    start_location POINT NOT NULL,
    end_station BINARY(16) NOT NULL,
    created_at TIMESTAMP NOT NULL,
    pod_id BINARY(16) NOT NULL,
    
    FOREIGN KEY (pod_id) REFERENCES pods(id),
    FOREIGN KEY (user_id) REFERENCES users(id),
    FOREIGN KEY (end_station) REFERENCES stations(id)
);

CREATE TABLE cargo_reservations(
    reservation_id BINARY(16) NOT NULL PRIMARY KEY,
    mass INT NOT NULL,
    width INT NOT NULL,
    height INT NOT NULL,
    length INT NOT NULL,
    note VARCHAR(512),
    
    FOREIGN KEY (reservation_id) REFERENCES reservations(id)
);

-- Insert mock data
INSERT INTO users (id, firstname, lastname, dob, role)
VALUES (
           UNHEX(REPLACE(UUID(), '-', '')),
           'Alice',
           'Johnson',
           '1990-05-15',
           'user'
       );

INSERT INTO stations (id, name, coords) 
VALUES (
            UNHEX(REPLACE(UUID(), '-', '')),
            'Romeo',
            POINT(48.8583, 2.2923)
       );

INSERT INTO stations (id, name, coords)
VALUES (
           UNHEX(REPLACE(UUID(), '-', '')),
           'Juliet',
           POINT(51.0263888889, 2.6377777778)
       );

INSERT INTO stations (id, name, coords)
VALUES (
        UNHEX(REPLACE(UUID(), '-', '')),
        'Marco',
        POINT(51.195583, 2.980861)
       );

INSERT INTO stations (id, name, coords)
VALUES (
           UNHEX(REPLACE(UUID(), '-', '')),
           'Aldegundis',
           POINT(51.219448, 4.402464)
       );

INSERT INTO stations (id, name, coords)
VALUES (
           UNHEX(REPLACE(UUID(), '-', '')),
           'Bravomagus',
           POINT(50.850346, 4.351721)
       );

INSERT INTO stations (id, name, coords)
VALUES (
           UNHEX(REPLACE(UUID(), '-', '')),
           'Gontimara',
           POINT(51.054342, 3.717424)
       );

INSERT INTO stations (id, name, coords)
VALUES (
           UNHEX(REPLACE(UUID(), '-', '')),
           'Leodicum',
           POINT(50.632557, 5.579666)
       );


INSERT INTO stations (id, name, coords)
VALUES (
           UNHEX(REPLACE(UUID(), '-', '')),
           'Bruggia',
           POINT(51.209348, 3.224700)
       );

-- Romeo
INSERT INTO pods (id, name, is_active, capacity, max_capacity, current_location) VALUES
    (UUID_TO_BIN(UUID()), 'Romeo Pod 1', TRUE, 20, 30, POINT(48.858600, 2.292300)),
    (UUID_TO_BIN(UUID()), 'Romeo Pod 2', TRUE, 20, 30, POINT(48.858300, 2.292600)),
    (UUID_TO_BIN(UUID()), 'Romeo Pod 3', TRUE, 20, 30, POINT(48.858000, 2.292300)),
    (UUID_TO_BIN(UUID()), 'Romeo Pod 4', TRUE, 20, 30, POINT(48.858300, 2.292000));

-- Juliet
INSERT INTO pods VALUES
    (UUID_TO_BIN(UUID()), 'Juliet Pod 1', TRUE, 20, 30, POINT(51.0266888889, 2.6377777778)),
    (UUID_TO_BIN(UUID()), 'Juliet Pod 2', TRUE, 20, 30, POINT(51.0263888889, 2.6380777778)),
    (UUID_TO_BIN(UUID()), 'Juliet Pod 3', TRUE, 20, 30, POINT(51.0260888889, 2.6377777778)),
    (UUID_TO_BIN(UUID()), 'Juliet Pod 4', TRUE, 20, 30, POINT(51.0263888889, 2.6374777778));
    
-- Marco
INSERT INTO pods VALUES
    (UUID_TO_BIN(UUID()), 'Marco Pod 1', TRUE, 20, 30, POINT(51.195883, 2.980861)),
    (UUID_TO_BIN(UUID()), 'Marco Pod 2', TRUE, 20, 30, POINT(51.195583, 2.981161)),
    (UUID_TO_BIN(UUID()), 'Marco Pod 3', TRUE, 20, 30, POINT(51.195283, 2.980861)),
    (UUID_TO_BIN(UUID()), 'Marco Pod 4', TRUE, 20, 30, POINT(51.195583, 2.980561));
    
-- Aldegundis
INSERT INTO pods VALUES
    (UUID_TO_BIN(UUID()), 'Aldegundis Pod 1', TRUE, 20, 30, POINT(51.219748, 4.402464)),
    (UUID_TO_BIN(UUID()), 'Aldegundis Pod 2', TRUE, 20, 30, POINT(51.219448, 4.402764)),
    (UUID_TO_BIN(UUID()), 'Aldegundis Pod 3', TRUE, 20, 30, POINT(51.219148, 4.402464)),
    (UUID_TO_BIN(UUID()), 'Aldegundis Pod 4', TRUE, 20, 30, POINT(51.219448, 4.402164));
    
-- Bravomagus
INSERT INTO pods VALUES
    (UUID_TO_BIN(UUID()), 'Bravomagus Pod 1', TRUE, 20, 30, POINT(50.850646, 4.351721)),
    (UUID_TO_BIN(UUID()), 'Bravomagus Pod 2', TRUE, 20, 30, POINT(50.850346, 4.352021)),
    (UUID_TO_BIN(UUID()), 'Bravomagus Pod 3', TRUE, 20, 30, POINT(50.850046, 4.351721)),
    (UUID_TO_BIN(UUID()), 'Bravomagus Pod 4', TRUE, 20, 30, POINT(50.850346, 4.351421));
    
-- Gontimara
INSERT INTO pods VALUES
    (UUID_TO_BIN(UUID()), 'Gontimara Pod 1', TRUE, 20, 30, POINT(51.054642, 3.717424)),
    (UUID_TO_BIN(UUID()), 'Gontimara Pod 2', TRUE, 20, 30, POINT(51.054342, 3.717724)),
    (UUID_TO_BIN(UUID()), 'Gontimara Pod 3', TRUE, 20, 30, POINT(51.054042, 3.717424)),
    (UUID_TO_BIN(UUID()), 'Gontimara Pod 4', TRUE, 20, 30, POINT(51.054342, 3.717124));
    
-- Leodicum
INSERT INTO pods VALUES
    (UUID_TO_BIN(UUID()), 'Leodicum Pod 1', TRUE, 20, 30, POINT(50.632857, 5.579666)),
    (UUID_TO_BIN(UUID()), 'Leodicum Pod 2', TRUE, 20, 30, POINT(50.632557, 5.579966)),
    (UUID_TO_BIN(UUID()), 'Leodicum Pod 3', TRUE, 20, 30, POINT(50.632257, 5.579666)),
    (UUID_TO_BIN(UUID()), 'Leodicum Pod 4', TRUE, 20, 30, POINT(50.632557, 5.579366));
    
-- Bruggia
INSERT INTO pods VALUES
    (UUID_TO_BIN(UUID()), 'Bruggia Pod 1', TRUE, 20, 30, POINT(51.209648, 3.224700)),
    (UUID_TO_BIN(UUID()), 'Bruggia Pod 2', TRUE, 20, 30, POINT(51.209348, 3.225000)),
    (UUID_TO_BIN(UUID()), 'Bruggia Pod 3', TRUE, 20, 30, POINT(51.209048, 3.224700)),
    (UUID_TO_BIN(UUID()), 'Bruggia Pod 4', TRUE, 20, 30, POINT(51.209348, 3.224400));