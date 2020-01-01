# [2.0.0](https://github.com/ashblue/fluid-dialogue/compare/v1.2.0...v2.0.0) (2020-01-01)


### Features

* **conditions:** added ability to access the parent node ([d4e7461](https://github.com/ashblue/fluid-dialogue/commit/d4e7461))


### BREAKING CHANGES

* **conditions:** Any existing custom conditions will need to convert `OnGetIsValid()` to
`OnGetIsValid(INode parent)`

# [1.2.0](https://github.com/ashblue/fluid-dialogue/compare/v1.1.1...v1.2.0) (2020-01-01)


### Features

* **nodes:** nodes now trigger a NodeEnter event when used ([55ed467](https://github.com/ashblue/fluid-dialogue/commit/55ed467))

## [1.1.1](https://github.com/ashblue/fluid-dialogue/compare/v1.1.0...v1.1.1) (2020-01-01)


### Bug Fixes

* **actions:** actions outside of the package now populate in the menu ([0645efb](https://github.com/ashblue/fluid-dialogue/commit/0645efb))

# [1.1.0](https://github.com/ashblue/fluid-dialogue/compare/v1.0.0...v1.1.0) (2019-08-21)


### Features

* **packages:** latest database and moved to unity event plus ([3b1b3c8](https://github.com/ashblue/fluid-dialogue/commit/3b1b3c8))

# 1.0.0 (2019-08-20)


### Bug Fixes

* **graph:** fix for ctrl click adding ([0bd2dcb](https://github.com/ashblue/fluid-dialogue/commit/0bd2dcb))
* **graph:** text selection no longer randomly stops working ([0805a04](https://github.com/ashblue/fluid-dialogue/commit/0805a04))
* **settings:** no longer crashes when trying to load ([1dd9215](https://github.com/ashblue/fluid-dialogue/commit/1dd9215))


### Features

* **docs:** initial documentation ([cbe7d63](https://github.com/ashblue/fluid-dialogue/commit/cbe7d63))
