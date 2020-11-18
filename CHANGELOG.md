# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]
### Added
- `Custom` match [#36]
- `None` match [#33]
- `Any` match [#31]
- `All` match [#29]
- `NotEqual` match using a comparer [#27]
- `Equal` match using a comparer [#25]
- `LessThanOrEqualTo` match using a comparer [#23]
- `LessThan` match using a comparer [#21]
- `GreaterThanOrEqualTo` match using a comparer [#19]
- `GreaterThan` match using a comparer [#17]
- `NotSame` match [#15]
- `Same` match [#13]
- `NotNull` match [#11]
- `NotEqual` match using an equality comparer [#9]
- `Equal` match using an equality comparer [#7]
- `Null` match [#5]
- `Never` match [#3]
- `Always` match [#1]

### Changed
- Prevent ambiguity of factory methods for matches that match for equality using names 'Equal'/'NotEqual' (use of equality comparer) and 'EqualTo'/'NotEqualTo' (use of comparer) [#38]

### Deprecated
- `Except` match that has only been introduced for compatibility when migrating from Delizious-Filtering component [#33]
