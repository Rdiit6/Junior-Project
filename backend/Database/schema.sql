-- ClimaPulse database schema (SQLite)
-- Modul 4, Database Design. Run once to create climapulse.db.

PRAGMA foreign_keys = ON;

CREATE TABLE UserProfile (
    profile_id                 INTEGER PRIMARY KEY AUTOINCREMENT,
    display_name               TEXT    NOT NULL CHECK (length(display_name) BETWEEN 1 AND 50),
    preferred_temperature_unit TEXT    NOT NULL DEFAULT 'C' CHECK (preferred_temperature_unit IN ('C', 'F')),
    preferred_locale           TEXT    NOT NULL DEFAULT 'id-ID' CHECK (length(preferred_locale) <= 10),
    created_at                 TEXT    NOT NULL DEFAULT (strftime('%Y-%m-%dT%H:%M:%SZ', 'now')),
    is_active                  INTEGER NOT NULL DEFAULT 1 CHECK (is_active IN (0, 1))
);

CREATE TABLE City (
    city_id        INTEGER PRIMARY KEY AUTOINCREMENT,
    name           TEXT    NOT NULL CHECK (length(name) BETWEEN 1 AND 100),
    country_code   TEXT    NOT NULL CHECK (length(country_code) = 2),
    admin_region   TEXT    CHECK (admin_region IS NULL OR length(admin_region) <= 100),
    latitude       REAL    NOT NULL CHECK (latitude BETWEEN -90 AND 90),
    longitude      REAL    NOT NULL CHECK (longitude BETWEEN -180 AND 180),
    timezone       TEXT    NOT NULL CHECK (length(timezone) <= 40),
    elevation_m    REAL,
    open_meteo_ref TEXT    CHECK (open_meteo_ref IS NULL OR length(open_meteo_ref) <= 40)
);

CREATE TABLE Watchlist (
    watchlist_id           INTEGER PRIMARY KEY AUTOINCREMENT,
    profile_id             INTEGER NOT NULL REFERENCES UserProfile (profile_id) ON DELETE CASCADE,
    city_id                INTEGER NOT NULL REFERENCES City (city_id) ON DELETE CASCADE,
    alert_threshold_zscore REAL    NOT NULL DEFAULT 2.0 CHECK (alert_threshold_zscore > 0),
    notes                  TEXT    CHECK (notes IS NULL OR length(notes) <= 500),
    added_at               TEXT    NOT NULL DEFAULT (strftime('%Y-%m-%dT%H:%M:%SZ', 'now')),
    UNIQUE (profile_id, city_id)
);

CREATE TABLE HistoricalObservation (
    observation_id INTEGER PRIMARY KEY AUTOINCREMENT,
    city_id        INTEGER NOT NULL REFERENCES City (city_id) ON DELETE CASCADE,
    metric_type    TEXT    NOT NULL CHECK (metric_type IN ('temperature_2m', 'precipitation', 'wind_speed_10m')),
    observed_at    TEXT    NOT NULL,
    value          REAL    NOT NULL,
    source         TEXT    NOT NULL DEFAULT 'open-meteo' CHECK (length(source) <= 30),
    fetched_at     TEXT    NOT NULL DEFAULT (strftime('%Y-%m-%dT%H:%M:%SZ', 'now')),
    UNIQUE (city_id, metric_type, observed_at)
);

CREATE TABLE Baseline (
    baseline_id         INTEGER PRIMARY KEY AUTOINCREMENT,
    city_id             INTEGER NOT NULL REFERENCES City (city_id) ON DELETE CASCADE,
    metric_type         TEXT    NOT NULL CHECK (metric_type IN ('temperature_2m', 'precipitation', 'wind_speed_10m')),
    month               INTEGER NOT NULL CHECK (month BETWEEN 1 AND 12),
    mean_value          REAL    NOT NULL,
    stddev_value        REAL    NOT NULL CHECK (stddev_value >= 0),
    sample_size         INTEGER NOT NULL CHECK (sample_size > 0),
    baseline_start_year INTEGER NOT NULL,
    baseline_end_year   INTEGER NOT NULL,
    computed_at         TEXT    NOT NULL DEFAULT (strftime('%Y-%m-%dT%H:%M:%SZ', 'now')),
    CHECK (baseline_start_year <= baseline_end_year),
    UNIQUE (city_id, metric_type, month)
);

CREATE TABLE Advisory (
    advisory_id    INTEGER PRIMARY KEY AUTOINCREMENT,
    metric_type    TEXT    NOT NULL CHECK (metric_type IN ('temperature_2m', 'precipitation', 'wind_speed_10m')),
    severity_tier  TEXT    NOT NULL CHECK (severity_tier IN ('watch', 'warning', 'extreme')),
    locale         TEXT    NOT NULL DEFAULT 'id-ID' CHECK (length(locale) <= 10),
    category       TEXT    NOT NULL CHECK (category IN ('health', 'climate')),
    title          TEXT    NOT NULL CHECK (length(title) <= 120),
    body           TEXT    NOT NULL CHECK (length(body) <= 2000),
    version        INTEGER NOT NULL DEFAULT 1 CHECK (version >= 1),
    effective_from TEXT    NOT NULL DEFAULT (strftime('%Y-%m-%dT%H:%M:%SZ', 'now')),
    UNIQUE (metric_type, severity_tier, locale, version)
);

CREATE TABLE AnomalyEvent (
    event_id                 INTEGER PRIMARY KEY AUTOINCREMENT,
    city_id                  INTEGER NOT NULL REFERENCES City (city_id) ON DELETE RESTRICT,
    baseline_id              INTEGER NOT NULL REFERENCES Baseline (baseline_id) ON DELETE RESTRICT,
    advisory_id              INTEGER REFERENCES Advisory (advisory_id) ON DELETE SET NULL,
    metric_type              TEXT    NOT NULL CHECK (metric_type IN ('temperature_2m', 'precipitation', 'wind_speed_10m')),
    severity_tier            TEXT    NOT NULL CHECK (severity_tier IN ('watch', 'warning', 'extreme')),
    started_at               TEXT    NOT NULL,
    ended_at                 TEXT,
    peak_at                  TEXT    NOT NULL,
    peak_observed_value      REAL    NOT NULL,
    peak_zscore              REAL    NOT NULL,
    baseline_mean_snapshot   REAL    NOT NULL,
    baseline_stddev_snapshot REAL    NOT NULL,
    status                   TEXT    NOT NULL DEFAULT 'active' CHECK (status IN ('active', 'closed')),
    created_at               TEXT    NOT NULL DEFAULT (strftime('%Y-%m-%dT%H:%M:%SZ', 'now')),
    CHECK (
        (status = 'active' AND ended_at IS NULL) OR
        (status = 'closed' AND ended_at IS NOT NULL AND ended_at >= started_at)
    )
);

CREATE INDEX idx_anomaly_city_metric_start ON AnomalyEvent (city_id, metric_type, started_at);
CREATE INDEX idx_anomaly_status            ON AnomalyEvent (status);
CREATE INDEX idx_watchlist_city            ON Watchlist (city_id);
