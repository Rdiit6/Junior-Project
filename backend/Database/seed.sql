-- Optional sample data, useful for screenshots of the tables.

INSERT INTO UserProfile (display_name, preferred_temperature_unit, preferred_locale)
VALUES ('Radit', 'C', 'id-ID');

INSERT INTO City (name, country_code, admin_region, latitude, longitude, timezone, elevation_m) VALUES
('Yogyakarta', 'ID', 'DI Yogyakarta',  -7.7956, 110.3695, 'Asia/Jakarta', 114),
('Jakarta',    'ID', 'DKI Jakarta',    -6.2088, 106.8456, 'Asia/Jakarta',   8),
('Tokyo',      'JP', 'Tokyo',          35.6762, 139.6503, 'Asia/Tokyo',    40);

INSERT INTO Watchlist (profile_id, city_id, alert_threshold_zscore, notes) VALUES
(1, 1, 2.0, 'Home city'),
(1, 2, 2.5, NULL);

INSERT INTO Baseline (city_id, metric_type, month, mean_value, stddev_value, sample_size, baseline_start_year, baseline_end_year) VALUES
(1, 'temperature_2m', 9, 26.4, 0.8, 900, 1995, 2024),
(2, 'temperature_2m', 9, 28.1, 0.7, 900, 1995, 2024);

INSERT INTO Advisory (metric_type, severity_tier, locale, category, title, body) VALUES
('temperature_2m', 'warning', 'id-ID', 'health',
 'Cuaca panas ekstrem',
 'Perbanyak minum air, hindari aktivitas luar ruangan pada siang hari, dan perhatikan kelompok rentan.');

INSERT INTO AnomalyEvent (city_id, baseline_id, advisory_id, metric_type, severity_tier, started_at, ended_at, peak_at,
                          peak_observed_value, peak_zscore, baseline_mean_snapshot, baseline_stddev_snapshot, status)
VALUES (1, 1, 1, 'temperature_2m', 'warning', '2026-09-18T05:00:00Z', '2026-09-19T10:00:00Z', '2026-09-18T07:00:00Z',
        29.1, 3.38, 26.4, 0.8, 'closed');
