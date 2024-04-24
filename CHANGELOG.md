# [3.0.0](https://github.com/ashblue/fluid-dialogue/compare/v2.6.0...v3.0.0) (2024-04-24)


### Bug Fixes

* **dialogue editor:** no longer fires an error when deleting list items ([96449e8](https://github.com/ashblue/fluid-dialogue/commit/96449e8e5197829b68f11b84010a9eb80a87aeae))


### Features

* added experimental nested graph playback restoration ([fb18251](https://github.com/ashblue/fluid-dialogue/commit/fb1825179ae46331067a340782d2d95b8dcc6cab))
* **custom nodes:** users now have necessary hooks to create their own custom nodes (experimental) ([09e8596](https://github.com/ashblue/fluid-dialogue/commit/09e8596e660f0195ada3e180f4b8b97fb7eb3e01))
* **graph creation:** can create new graphs externally now ([e92a3ca](https://github.com/ashblue/fluid-dialogue/commit/e92a3cae3192108045fc588356bd2ae9e68274bf))
* **new node:** note nodes can now be added to graphs ([45585fe](https://github.com/ashblue/fluid-dialogue/commit/45585feae62a7fe23e100750efb1165f39e4fb5a))
* **runtime gameobjects:** replaced game object overrides with a send message action API ([cef3f97](https://github.com/ashblue/fluid-dialogue/commit/cef3f97d21e7fdf0ca888f0b26d5b194e21b2fad))


### BREAKING CHANGES

* **runtime gameobjects:** You should remove all references to GameObject overrides, be ready to replace the
game object names with paths (write them down). After upgrading you'll need to swap or remove all
references to game object overrides from your project.
