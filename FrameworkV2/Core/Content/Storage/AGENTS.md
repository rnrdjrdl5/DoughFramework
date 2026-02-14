# Storage Content Notes

This folder provides a storage Ability that delegates Core Save/Load requests to external systems.
The save payload is stored in a Dictionary inside the Ability, and external systems
persist or update the Dictionary during Save/Load events. Actual save/load processing is TBD.
Keys use the type FullName by default.
