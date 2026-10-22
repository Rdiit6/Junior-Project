# ClimaPulse: Design References

Status: draft for team review. Last updated 2026-10-22.

This file collects design references for the ClimaPulse desktop app (a WPF window whose screens are Blazor components), from the start screen to the screens inside the app, plus the project's GitHub Page. It also proposes a starter color system and page layouts so the team has something concrete to react to. Requirement IDs (FR-xx) point to `docs/SRD.md`.

**Link status.** *Verified* means the link appeared in a live web search on 2026-10-17. *Check* means it is a well-known page I could not open from my environment, so open it once before the team relies on it. Nobody on the team should copy a design. Take the idea, then draw your own version.

---

## 1. Decide the visual direction first

Pick one direction before anyone opens Figma. Mixed directions make a project look unfinished.

| Direction | Looks like | Pros | Cons |
|---|---|---|---|
| **A. Data-first and calm** (recommended) | White or dark neutral canvas, big numbers, one accent color, color used only for severity | Matches the product idea (explain how unusual a reading is). Easy to keep accessible. Charts stay readable. Fast to build with MudBlazor. | Can look plain if the GitHub Page has no strong hero. |
| **B. Friendly weather app** | Gradients, large weather illustrations, rounded cards | Instantly familiar and attractive in screenshots. | Decoration competes with the risk badges. Gradients make contrast hard to control. Looks like a forecast app, not an anomaly tool. |
| **C. Operations dashboard** | Dark theme, dense panels, map in the center | Feels serious and powerful. Good for many cities. | Hardest to build. Needs a map. Intimidating for first-time users. |

**Recommendation:** A for the app, with a bolder GitHub Page borrowed from the data-storytelling world (section 3.1). Decide by team vote and record the answer here: `Chosen direction: ____________`.

---

## 2. Screen inventory

| # | Screen | Purpose | SRD |
|---|---|---|---|
| S1 | Start screen and profile picker | Pick or create a profile. On first run, explain the idea in one sentence and offer "Add your first city" | FR-29, FR-01 |
| S2 | Add city (search) | Find a city, confirm, add to watchlist | FR-01, FR-02 |
| S3 | Dashboard | See every watched city and its risk at a glance | FR-17, FR-18 |
| S4 | City detail | Understand one city: value, baseline, z-score, history, advisory | FR-19, FR-20 |
| S5 | Anomaly ledger | Browse and export past events | FR-26, FR-27 |
| S6 | Settings | Unit, language, thresholds, theme | FR-04, FR-05, FR-21 |
| S7 | System states | Loading, empty, error, partial failure | NFR-04, NFR-17 |
| S8 | GitHub Page (outside the app) | Present the product, the team and the demo download. Required by Module 1 | Module 1 |

---

## 3. References per screen

### 3.0 Start screen and profile picker (S1)

- Pattern: a centered list of profile cards (initial letter, display name) and a "New profile" card. The last used profile has focus, so Enter continues.
- New profile asks only for a display name. Unit and language default to Celsius and Indonesian and can be changed in Settings.
- First run (no profiles, no cities): one sentence about what ClimaPulse does, then "Create your profile" and "Add your first city". No tour, no slides.
- Pitfall: do not make it look like a login screen. There is no password, and the SRD says so.

### 3.1 GitHub Page (S8) and first-run message

A desktop app has no landing page. The landing-page ideas below now serve the GitHub Page that Module 1 requires (published from `docs/` on `main`) and, in a short form, the first-run message on S1.

**What the page must do:** say what ClimaPulse is, show one real example of an anomaly, explain why a z-score is better than a plain forecast, and offer one clear button.

**Recommended section order**
1. Hero: one sentence plus a live or sample anomaly card (city, today's value, "+3.4 sigma, Warning").
2. How it works in three steps: compare with 30 years of history, measure how unusual, get advice.
3. A sample dashboard screenshot.
4. Why it matters: a forecast says "35 C", ClimaPulse says "this is extreme for Yogyakarta in October".
5. Ledger preview and export.
6. Final call to action (download the demo build from the GitHub Release), the team members, and a footer with data attribution to Open-Meteo.

**References**

| Reference | Take this idea | Avoid | Link | Status |
|---|---|---|---|---|
| Copernicus Climate Pulse | One page that answers "how unusual is today" with a single chart and a plain headline. Closest match to our product idea. | Its scope is mainly global, ours is per city. | https://climate.copernicus.eu/climate-pulse-tool-take-temperature-our-planet-glance | Verified |
| Climate Pulse award showcase | Why the design won: clarity over decoration. | Nothing to avoid, read it as a case study. | https://www.informationisbeautifulawards.com/showcase/7204-climate-pulse | Verified |
| Warming Stripes (Show Your Stripes) | A hero made of colored vertical stripes, instantly recognizable. A stripes strip could show a city's anomaly by year. | Do not copy the exact stripes artwork for a brand mark. It is widely used, so a copy looks unoriginal. | https://wmo.int/node/25831 (explanation), https://showyourstripes.info (tool) | Verified (WMO) / Check |
| Our World in Data | Chart-first storytelling with a short headline above each chart and the source below it. | Dense multi-series charts are too heavy for a dashboard card. | https://ourworldindata.org/owid-grapher | Verified |
| NOAA Climate.gov Data Snapshots | Simple anomaly maps with a clear legend and one-sentence caption. | Map-heavy pages need a map library. | https://www.climate.gov/media/14102 | Verified |
| Weather UI roundups (InVision, Subframe, Dribbble stories) | Hero layouts and typography for weather products. | Gradients and ornamental icons that hide the risk level (direction B). | https://invisionapp.com/blog/weather-app-ui-examples , https://www.subframe.com/tips/weather-website-design-examples , https://dribbble.com/stories/2020/05/25/weather-app-concepts | Verified |
| Land-book and Awwwards galleries | Current landing page layouts in general. Search "data", "climate", "dashboard". | Heavy animation and effects that a simple GitHub Pages site cannot carry well. | https://land-book.com , https://www.awwwards.com | Check |

**Pros and cons of a hero chart versus a hero illustration.** A chart (stripes or an anomaly card) proves the product on the first screen and costs little to build. An illustration looks friendly but says nothing about what the product does, and needs someone who can draw. Use the chart.

### 3.2 Add city and search (S2)

- Pattern: one search box, results listed with city, region and country code, a single "Add" button per result.
- Show the new city's coordinates and time zone in the result so duplicate names (many city names exist in several countries) are easy to tell apart.
- Reference: Windy and Zoom Earth are worth a look for how they handle location search. Links: https://www.windy.com , https://zoom.earth (Check).
- Pitfall: do not require the user to understand latitude and longitude. Search by name only.

### 3.3 Dashboard (S3)

**Layout idea:** a card per city in a responsive grid, sorted by severity with the worst first. Each card shows city name, a large risk badge, the key metric with its z-score, and the time of last update. A filter chip row at the top can limit the view to one metric.

**References**

| Reference | Take this idea | Avoid | Link | Status |
|---|---|---|---|---|
| Dashboard Design Patterns (IEEE VIS 2022 paper) | A research catalogue of real dashboard layouts, useful to name and justify the layout you pick. | Academic wording, skim the figures. | https://arxiv.org/abs/2205.00757 | Verified |
| IBM Carbon data visualization | Rules for chart types, titles, legends and tooltips that work in dashboards. | Carbon's own component style, since we use MudBlazor. | https://carbondesignsystem.com/data-visualization/chart-types and https://ibm.com/design/language/data-visualization/overview | Verified |
| Visa dynamic table pattern | A table pattern from a large design system. Read how it handles sort, filter and layout, then apply it to the ledger. | Its visual style. | https://design.visa.com/patterns/dynamic-table | Verified |
| Windy / Zoom Earth / earth.nullschool | A full-screen map as a second view of the same data. | Making the map the only view. A list is faster to scan for 10 cities. | https://www.windy.com , https://zoom.earth , https://earth.nullschool.net | Check |
| Climate Reanalyzer | Plain, fast anomaly maps and daily charts, a good model of "serious but readable". | Its utilitarian styling, which is too bare for the GitHub Page. | https://climatereanalyzer.org/wx/todays-weather | Verified |

**Rules for the dashboard**
- Worst city first. A user opening the app wants to see what is wrong.
- Badge = color + icon + word. Never color alone.
- A card shows at most one big number, one badge and one line of context.
- Show "last updated" and a refresh button on every card group.
- Partial failure: if one city fails to load, show an error on that card only.

### 3.4 City detail (S4)

Content order: header (name, region, badge), today's reading against its baseline (a bell curve or a bar with a marked mean and one and two standard deviation bands), the advisory panel, a recent history chart, and a small table of this city's past events.

The bell curve with a marker is the clearest way to teach a z-score to a non-technical user. Reference for z-score and anomaly charts: NOAA Data Snapshots and Climate Pulse above.

NWS HeatRisk is a useful model for the advisory panel because it turns a number into a level and then into advice for specific groups of people: https://www.wpc.ncep.noaa.gov/heatrisk/understanding.html (Verified).

### 3.5 Anomaly ledger (S5)

- A table with filter chips (city, metric, tier, status, date range), newest first, paged, with an "Export CSV" and "Export JSON" button in the table header.
- Row: tier badge, city, metric, started, ended or "Active", peak value, peak z-score.
- Row click opens a drawer with the snapshot values (baseline mean and standard deviation at the time).
- Reference for table behavior: Visa pattern above.

### 3.6 Settings (S6)

Keep it short: temperature unit, language, theme, and the per-city alert threshold (that one can live on the city detail page instead). Explain the threshold in plain words, for example "Alert me when a reading is 2 or more standard deviations from normal."

### 3.7 System states (S7)

| State | Show | Reference |
|---|---|---|
| Loading | Skeleton cards with the same shape as real cards | https://www.saasui.design/blog/saas-loading-skeleton-ux-patterns (Verified) |
| Empty watchlist | A friendly message and one button, "Add your first city" | same article |
| Open-Meteo down | A banner: "Live data is unavailable. Showing last known readings from 14:05." | n/a |
| Partial failure | Error on the failing card only, retry button | n/a |

---

## 4. Warning-level conventions to follow

Official services already trained people to read yellow, orange and red as rising danger. Reusing that convention is safer than inventing colors.

| Source | What it teaches | Link | Status |
|---|---|---|---|
| MeteoAlarm (Europe) | Four levels: green (no warning), yellow, orange, red. Our four tiers map directly. | https://meteoalarm.readthedocs.io/en/latest/warnings.html | Verified |
| BMKG (Indonesia) | Alert levels named Waspada, Siaga and Awas, used in the daily early warning. Indonesian users already know these words, so use them in the Indonesian UI. | https://news.detik.com/berita/d-8112789/cara-cek-informasi-cuaca-real-time-via-situs-dan-aplikasi-bmkg (how to read BMKG's site and app), official site https://www.bmkg.go.id | Verified (news) / Check (official) |
| NWS HeatRisk (USA) | A 0 to 4 scale from green to magenta, tied to health advice. | https://www.wpc.ncep.noaa.gov/heatrisk/understanding.html | Verified |

**Proposed tier names**

| Tier (code and database) | English label | Indonesian label |
|---|---|---|
| Normal (not stored) | Normal | Normal |
| `watch` | Watch | Waspada |
| `warning` | Warning | Siaga |
| `extreme` | Extreme | Awas |

Confirm with the team that matching BMKG's three words is acceptable. **Pro:** instantly understood in Indonesia. **Con:** BMKG levels are defined by official criteria, and ours are statistical, so the UI must say "statistical anomaly" and must not claim to be an official warning.

---

## 5. Proposed starter design tokens

These are proposals to react to, not decisions. Contrast ratios were computed with the WCAG formula on 2026-10-17.

### 5.1 Severity colors

Badge style: tinted background, dark text, icon, and word. That keeps text contrast above 6.9:1 in both themes.

| Tier | Icon (shape differs) | Light badge (text on tint) | Contrast | Dark badge (text on tint) | Contrast |
|---|---|---|---|---|---|
| Normal | circle with check | `#14532D` on `#DCFCE7` | 8.3 | `#BBF7D0` on `#14532D` | 7.5 |
| Watch | eye | `#78350F` on `#FEF3C7` | 8.2 | `#FEF08A` on `#713F12` | 7.5 |
| Warning | triangle | `#7C2D12` on `#FFEDD5` | 8.2 | `#FED7AA` on `#7C2D12` | 6.9 |
| Extreme | octagon with exclamation | `#7F1D1D` on `#FEE2E2` | 8.2 | `#FECACA` on `#7F1D1D` | 6.9 |

Chart and map marks need at least 3:1 against the page background (WCAG non-text contrast), so they use different values from the badges:

| Tier | Mark on light background | Contrast | Mark on dark background (`#0F172A`) | Contrast |
|---|---|---|---|---|
| Normal | `#15803D` | 5.0 | `#16A34A` | 5.4 |
| Watch | `#A16207` | 4.9 | `#EAB308` | 9.3 |
| Warning | `#C2410C` | 5.2 | `#F97316` | 6.4 |
| Extreme | `#B91C1C` | 6.5 | `#DC2626` | 3.7 |

A common mistake is to use the bright yellow and orange on a white page. Those two fail the 3:1 rule there (1.9 and 2.8), which is why the light-mode set is darker.

Yellow, orange and red are hard to tell apart for people with red-green color blindness. The icon shape and the word on every badge are what make the tiers readable for them. Do not remove those to make the UI "cleaner".

### 5.2 Neutrals and accent

| Token | Light | Dark | Contrast note |
|---|---|---|---|
| Page background | `#FFFFFF` | `#0F172A` | |
| Body text | `#0F172A` | `#E2E8F0` | 17.9 and 14.5 |
| Muted text | `#475569` | `#94A3B8` | 7.6 and 7.0 |
| Accent (links, primary button) | `#0369A1` | `#38BDF8` | 5.9 and 8.3 |

### 5.3 Typography and spacing

- Typeface: Inter, or the system sans-serif stack. **Pro:** very readable at small sizes and free (Google Fonts). **Con:** very common, so the look comes from layout and color, not the font.
- Sizes: 14 or 16 px body, 12 px labels, 28 to 40 px for the main number on a card.
- Use tabular (fixed-width) digits for numbers so values do not jump when they update.
- Spacing scale: 4, 8, 12, 16, 24, 32, 48 px.
- Window sizes: design at 1280 x 800, check the minimum 1024 x 640 and full HD 1920 x 1080 (SRD UI-1). Also check Windows display scaling at 125 and 150 percent, which most laptops use.
- Theme: light first, dark second (FR-21). MudBlazor supports both through its theme object.

### 5.4 Chart color rules

- Use a sequential palette for one measure (for example temperature) and a diverging palette for anomalies, with the neutral midpoint at zero.
- Warm colors above normal and cool colors below normal.
- Check every palette with a color blindness simulator before using it.
- References: Datawrapper colors guide https://www.datawrapper.de/academy/what-to-consider-when-choosing-colors-for-data-visualization (Verified), Datawrapper on color blindness https://www.datawrapper.de/blog/colorblindness-part2 (Verified), ColorBrewer https://colorbrewer2.org (Check), an article about the author of ColorBrewer https://www.r-bloggers.com/2014/12/cindy-brewer-helping-you-choose-better-color-scales-for-maps/ (Verified), Carbon color palettes https://v10.carbondesignsystem.com/data-visualization/color-palettes/ (Verified).

---

## 6. Wireframe sketches

### Dashboard (desktop)

```
+----------------------------------------------------------------------+
| ClimaPulse      Dashboard   Ledger   Settings        [Add city] [dark]|
+----------------------------------------------------------------------+
| Your cities (3)    Filter: [All] [Temperature] [Rain] [Wind]  Refresh |
| Last updated 14:05                                                    |
+--------------------------+--------------------------+----------------+
| Jakarta         [! Siaga]| Yogyakarta     [~ Waspada]| Tokyo [ok Normal]|
|  34.8 C   +3.2 sigma     |  33.1 C   +2.4 sigma     |  18.2 C  +0.3  |
|  Normal for Oct: 31.0 C  |  Normal for Oct: 30.0 C  |  Normal: 18.0  |
|  [ bell curve marker ]   |  [ bell curve marker ]   |  [ bar ]       |
+--------------------------+--------------------------+----------------+
```

### City detail (desktop)

```
< Back   Jakarta, ID                                     [! Siaga]
+---------------------------------------+---------------------------+
| Temperature today vs 30-year normal   | Advice (Health | Climate)  |
| [ bell curve, marker at +3.2 sigma ]  | Cuaca panas ekstrem ...    |
| Normal 31.0  Std dev 1.2  Today 34.8  | - Drink water often        |
+---------------------------------------+ - Avoid noon sun           |
| Last 14 days  [ line chart + band ]   +---------------------------+
+---------------------------------------+ Past events for this city  |
                                        | 2026-09-18  Warning  Closed|
```

### Ledger (desktop)

```
Filters: [City v] [Metric v] [Tier v] [Status v] [Date range]   [CSV] [JSON]
+------+-----------+-------------+-----------+----------+--------+
| Tier | City      | Metric      | Started   | Ended    | Peak z |
+------+-----------+-------------+-----------+----------+--------+
| [!]  | Jakarta   | Temperature | 10-16 06h | Active   | 3.4    |
| [~]  | Tokyo     | Temperature | 09-18 05h | 09-19 10h| 3.4    |
+------+-----------+-------------+-----------+----------+--------+
                                              < 1 2 3 >
```

In a narrow window (below about 900 px wide) the filter row collapses behind a "Filters" button.

---

## 7. Tools, kits and assets

### Component libraries for Blazor

| Option | Pros | Cons | Link | Status |
|---|---|---|---|---|
| MudBlazor | Free, many components, theming for light and dark, big community | Basic charts, occasional breaking changes between major versions. Blazor Hybrid support not confirmed yet, test it first (`docs/references-desktop-architecture.md`, section 1E) | https://github.com/MudBlazor/MudBlazor | Verified |
| Radzen Blazor | Rich data grid and charts, free tier | Heavier look, some features paid | https://www.radzen.com/blazor-components/ | Check |
| Blazor admin template roundup | Quick look at what ready-made dashboards look like | Templates often cost money and bring a lot of unused code, so use them as inspiration only | https://adminlte.io/blog/blazor-admin-dashboard-templates/ | Verified |

### Icons and illustrations

- Meteocons: free weather icons, including animated ones. https://github.com/basmilius/meteocons (Verified). Check the license text in the repository before using them in the project.
- Material Symbols or Lucide for interface icons (menu, filter, download). https://fonts.google.com/icons and https://lucide.dev (Check).
- Use the severity icons from section 5.1 for badges, and weather icons only as decoration on detail pages.

### Figma starting points

- A free weather app listing: https://mercier.gumroad.com/l/weather-app-free (Verified listing). Free kit roundup: https://designerup.co/blog/5-free-figma-ui-kits-to-jump-start-your-designs/ (Verified).
- Figma Community search for "weather dashboard" and "data dashboard": https://www.figma.com/community (Check).
- Check each kit's license before using anything in the final project.

---

## 8. Accessibility checklist for designers

Reference for the numbers: Texas DIR color contrast guide https://dir.texas.gov/txds/accessibility/color-contrast/ (Verified) and the WCAG 2.2 pages "Contrast (Minimum)" and "Use of Color" on https://www.w3.org/WAI/WCAG22/Understanding/ (Check).

- [ ] Normal text at least 4.5:1 against its background, large text at least 3:1.
- [ ] Chart marks, icons and borders at least 3:1 against what is behind them.
- [ ] No meaning by color alone: every tier also has an icon shape and a word.
- [ ] Every interactive element is reachable by keyboard and has a visible focus ring.
- [ ] Click targets at least 24 by 24 px (WCAG 2.2 minimum), 32 px or more preferred, and still usable at 150 percent display scaling.
- [ ] Charts have a text alternative (a sentence or a data table).
- [ ] Text can grow to 200 percent without the layout breaking.
- [ ] Animations are short and can be switched off for people who prefer reduced motion.

---

## 9. How the team should use this file

1. Each member collects **5 screenshots** that they like, each with one sentence about what to take from it (not "looks nice"). Put them in a shared Figma file or a folder.
2. Vote on the direction in section 1.
3. One person builds the tokens from section 5 in MudBlazor's theme and in Figma so designers and developers use the same values.
4. Draw the S3 dashboard and S4 city detail first. Those two screens carry the demo.
5. Test the severity badges with a color blindness simulator before building the other screens.

Open decision for the team: who owns design (one lead, so choices stay consistent) and who reviews accessibility.
