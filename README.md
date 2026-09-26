# ParticleStack

**ParticleStack** is a lightweight, code-first 2D particle system for Unity built around simple, modular components, GPU-instanced rendering, and easily programmable particle behaviour.

> Particle effects without the giant particle system.

**Current version: v0.4.0**

---

## Overview

ParticleStack is designed as a smaller, clearer alternative for 2D particle effects when you want direct control over how particles are emitted, simulated, and modified.

Instead of putting every possible setting into one large component, ParticleStack separates an effect into focused pieces:

- **PSEmitter** — owns, simulates, and renders particles.
- **PSEmission** — decides when particles are created.
- **PSShape** — decides where particles spawn and which direction they initially travel.
- **PSBehaviour** — modifies particles when they spawn or while they are alive.

Built-in ParticleStack components use the same public extension system available to user code, so custom shapes, emissions, and behaviours fit into the workflow without modifying the core package.

---

## v0.4.0

v0.4.0 expands ParticleStack from a functional particle renderer into a much more complete Unity tool.

### Highlights

- GPU-instanced 2D particle rendering.
- Data-only particles with no GameObject, MonoBehaviour, Rigidbody2D, or SpriteRenderer per particle.
- Burst and continuous emission.
- Circle, Line, Cone, and Box emission shapes.
- Random colour, lifetime, scale, and speed.
- Gravity, force, and drag behaviours.
- Scale, colour, and velocity over time.
- Lifetime-driven sprite animation.
- Extensible Shape, Emission, and Behaviour APIs.
- Custom PSEmitter inspector.
- Collapsible inspector sections.
- Live runtime particle and GPU batch information.
- **Add Shape**, **Add Emission**, and **Add Behaviour** menus that automatically discover derived classes.
- Hierarchy creation menu for quickly creating a ready-to-use ParticleStack particle system.
- Improved behaviour compatibility and per-particle state handling.

---

## Philosophy

ParticleStack is not intended to reproduce every feature of Unity's built-in Particle System.

The goal is to provide a focused 2D particle framework that is:

- **Lightweight** — particles are plain data rather than individual Unity objects.
- **Explicit** — behaviour is driven by small components with clear responsibilities.
- **Extensible** — custom effects can be built by deriving from the same base classes as built-in functionality.
- **Programmer-first** — systems remain easy to understand, modify, and drive from code.
- **Modular** — add only the behaviour an effect actually needs.
- **Performance-conscious** — particles are rendered using GPU instancing.

---

## Core Architecture

A ParticleStack effect is assembled from a small set of components:

```text
ParticleStack Particle System
│
├── PSEmitter
│
├── PSShape
│   └── e.g. PSCircleShape
│
├── PSEmission
│   └── e.g. PSOngoingEmission
│
└── PSBehaviour
    ├── e.g. PSGravityBeh
    ├── e.g. PSRandomScaleBeh
    └── e.g. PSColourOverTimeBeh
```

### PSEmitter

`PSEmitter` owns the particle buffer and is responsible for:

- particle storage
- particle lifetime
- behaviour execution
- movement and rotation
- particle removal
- GPU-instanced rendering
- per-particle colour
- per-particle sprite UV data

Particles are stored in a preallocated array and active particles remain packed at the beginning of the buffer.

Dead particles are removed using swap-with-last removal rather than shifting the entire array.

---

## Particle Rendering

ParticleStack does not create a GameObject or SpriteRenderer for every particle.

Instead, particle data is converted into GPU instance data and rendered in batches.

Current instance data includes:

- position
- rotation
- scale
- colour
- sprite UV rectangle

ParticleStack currently batches up to **1023 particles per draw call** using Unity's instanced mesh rendering.

---

## Emission

### PSBurstEmission

Emits a set number of particles at once.

Useful for:

- explosions
- impacts
- hit effects
- destruction effects
- one-shot visual effects

### PSOngoingEmission

Continuously emits particles using an accumulator-based emission system.

This allows emission rates to remain consistent across different frame rates and supports rates greater than the current frame rate.

Useful for:

- fire
- smoke
- trails
- ambient effects
- continuous emitters

---

## Shapes

ParticleStack v0.4.0 includes:

### PSCircleShape

Emits particles using a configurable radial range.

A radius of zero can act as a point emitter, while a range can produce rings or radial bands.

### PSLineShape

Spawns particles along a line using the emitter transform for orientation.

### PSConeShape

Emits particles within a configurable cone angle and radius.

### PSBoxShape

Spawns particles along the edges of a box with outward-facing directions.

---

## Behaviours

### Randomisation

- `PSRandomColourBeh`
- `PSRandomLifetimeBeh`
- `PSRandomScaleBeh`
- `PSRandomSpeedBeh`

### Motion

- `PSGravityBeh`
- `PSForceBeh`
- `PSDragBeh`
- `PSVelocityOverTimeBeh`

### Lifetime Effects

- `PSScaleOverTimeBeh`
- `PSColourOverTimeBeh`

### Animation

- `PSAnimateSpriteBeh`

`PSAnimateSpriteBeh` selects animation frames from the particle's lifetime.

For example:

```text
8 frames
2 second lifetime

8 / 2 = 4 frames per second
```

This keeps the animation duration naturally matched to the life of each particle.

Animation frames should use the same source texture as the emitter sprite.

---

## Editor Workflow

ParticleStack includes a custom editor for `PSEmitter`.

The inspector keeps the emitter compact by grouping settings into collapsible sections and adds runtime information while the game is running.

```text
▼ Emitter Settings

▼ Particle Settings

▼ Rendering Settings

[ Add Shape ]
[ Add Emission ]
[ Add Behaviour ]

▼ Runtime Info
    Active Particles
    GPU Batches
```

The Add menus use Unity's type discovery system, so custom classes derived from `PSShape`, `PSEmission`, or `PSBehaviour` automatically appear without modifying the editor.

---

## Creating a Particle System

ParticleStack can be created directly from Unity's GameObject / Hierarchy menu:

```text
GameObject
└── ParticleStack
    └── Particle System
```

This creates a ready-to-configure ParticleStack GameObject with the core components already attached.

---

## Creating Custom Behaviours

Create a class derived from `PSBehaviour`:

```csharp
using UnityEngine;

public class PSExampleBeh : PSBehaviour
{
    public override void OnParticleSpawn(ref PSParticle particle)
    {
        // Called once when this particle is emitted.
    }

    public override void UpdateParticle(
        ref PSParticle particle,
        float deltaTime
    )
    {
        // Called every simulation update while the particle is alive.
    }
}
```

Once compiled, the behaviour automatically appears under:

```text
Add Behaviour
```

in the PSEmitter inspector.

---

## Creating Custom Shapes

Derive from `PSShape` and provide spawn position and initial direction:

```csharp
using UnityEngine;

public class PSExampleShape : PSShape
{
    public override void GetSpawnData(
        out Vector2 position,
        out Vector2 direction
    )
    {
        position = transform.position;
        direction = transform.up;
    }
}
```

The new shape automatically appears under **Add Shape**.

---

## Creating Custom Emission Types

Derive from `PSEmission` and use the shared particle-emission workflow provided by the base class.

Custom emission components automatically appear under **Add Emission**.

---

## Performance

ParticleStack is designed to avoid the overhead of a Unity object per particle.

The main runtime costs are instead:

- particle simulation
- behaviour execution
- instance matrix generation
- draw calls
- GPU overdraw

This makes ParticleStack particularly suited to effects containing large numbers of simple 2D particles.

---

## Current Scope

ParticleStack currently focuses on:

- 2D particles
- code-driven effects
- modular particle logic
- GPU-instanced rendering
- simple and extensible authoring

It deliberately avoids trying to match every feature of Unity's built-in Particle System.

If an effect needs specialised behaviour, ParticleStack is designed so that functionality can be added as a small custom component rather than expanding the core system indefinitely.

---

## Roadmap

### v0.5.0 — Validation & Safety

The next planned release focuses on editor-side error checking and warnings.

The goal is to make invalid or potentially confusing configurations obvious before they become runtime problems, similar to the validation and editor feedback added during FrameStack's later development.

Planned areas include:

- missing or incompatible resources
- invalid particle settings
- conflicting component configurations
- animation setup validation
- clearer editor feedback
- safer setup and configuration

---

## Requirements

ParticleStack is built for Unity and uses GPU instanced mesh rendering.

The included material should use the ParticleStack instanced shader.

---

## Author

**Ethan Gerty**

ParticleStack is part of the Stack family of Unity development tools alongside **FrameStack**.

---

## Version

**ParticleStack v0.4.0**
