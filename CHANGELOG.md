# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]
### Added
- `GreaterThanOrEqualTo` match for comparable types [[#50](https://github.com/oliverzick/Delizious-Toolkit/issues/50)]
- `GreaterThan` match for comparable types [[#48](https://github.com/oliverzick/Delizious-Toolkit/issues/48)]
- `NotEqualTo` match for comparable types [[#46](https://github.com/oliverzick/Delizious-Toolkit/issues/46)]
- `EqualTo` match for comparable types [[#44](https://github.com/oliverzick/Delizious-Toolkit/issues/44)]
- `NotEqual` match for equatable types [[#42](https://github.com/oliverzick/Delizious-Toolkit/issues/42)]
- `Equal` match for equatable types [[#40](https://github.com/oliverzick/Delizious-Toolkit/issues/40)]
- `Custom` match [[#36](https://github.com/oliverzick/Delizious-Toolkit/issues/36)]
- `None` match [[#33](https://github.com/oliverzick/Delizious-Toolkit/issues/33)]
- `Any` match [[#31](https://github.com/oliverzick/Delizious-Toolkit/issues/31)]
- `All` match [[#29](https://github.com/oliverzick/Delizious-Toolkit/issues/29)]
- `NotEqual` match using a comparer [[#27](https://github.com/oliverzick/Delizious-Toolkit/issues/27)]
- `Equal` match using a comparer [[#25](https://github.com/oliverzick/Delizious-Toolkit/issues/25)]
- `LessThanOrEqualTo` match using a comparer [[#23](https://github.com/oliverzick/Delizious-Toolkit/issues/23)]
- `LessThan` match using a comparer [[#21](https://github.com/oliverzick/Delizious-Toolkit/issues/21)]
- `GreaterThanOrEqualTo` match using a comparer [[#19](https://github.com/oliverzick/Delizious-Toolkit/issues/19)]
- `GreaterThan` match using a comparer [[#17](https://github.com/oliverzick/Delizious-Toolkit/issues/17)]
- `NotSame` match [[#15](https://github.com/oliverzick/Delizious-Toolkit/issues/15)]
- `Same` match [[#13](https://github.com/oliverzick/Delizious-Toolkit/issues/13)]
- `NotNull` match [[#11](https://github.com/oliverzick/Delizious-Toolkit/issues/11)]
- `NotEqual` match using an equality comparer [[#9](https://github.com/oliverzick/Delizious-Toolkit/issues/9)]
- `Equal` match using an equality comparer [[#7](https://github.com/oliverzick/Delizious-Toolkit/issues/7)]
- `Null` match [[#5](https://github.com/oliverzick/Delizious-Toolkit/issues/5)]
- `Never` match [[#3](https://github.com/oliverzick/Delizious-Toolkit/issues/3)]
- `Always` match [[#1](https://github.com/oliverzick/Delizious-Toolkit/issues/1)]

### Changed
- Prevent ambiguity of factory methods for matches that match for equality using names 'Equal'/'NotEqual' (use of equality comparer) and 'EqualTo'/'NotEqualTo' (use of comparer) [[#38](https://github.com/oliverzick/Delizious-Toolkit/issues/38)]

### Deprecated
- `Except` match that has only been introduced for compatibility when migrating from Delizious-Filtering component [[#33](https://github.com/oliverzick/Delizious-Toolkit/issues/33)]
