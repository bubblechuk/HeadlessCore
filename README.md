# HeadlessCore

**HeadlessCore (HCore)** is a C# / .NET library designed for managing RPG/JRPG game logic. Built with Hexagonal Architecture principles, it isolates core mechanics from the rendering layer, offering a decoupled and flexible foundation for game projects.

## Key Features

- **Hexagonal Architecture** — HCore acts purely as the business logic engine (stats, actions, effects, combat rules) with zero UI dependencies. It can be easily integrated into game engines like Unity, or framework engines like Raylib and MonoGame.
- **Extensibility** — Ships with base classes for characters, actions, and effects. You can extend base classes or introduce custom domain entities as needed.
- **Data-Driven Architecture** — Entity, skill, and effect instantiation is driven by JSON files handled through internal factories. Custom object types can be integrated by extending factory logic.
- **Localization** — Built-in localizer powered by JSON translation files for multi-language support.
- **Save System** — Simple Save Manager for serializing and loading game state via JSON.

## Roadmap

- World Map system and state management
- Base implementations for Towns and Shops
- Visual editor (desktop/web) for JSON file generation
- Demo application showcasing HCore integration