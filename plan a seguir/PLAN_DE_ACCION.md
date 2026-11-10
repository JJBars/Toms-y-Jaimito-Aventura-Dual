# 📋 Plan de Acción — Tomás y Jaimito Aventura Dual

**Objetivo:** Convertir el prototipo actual en un juego de puzles y plataformas cooperativo para 2 jugadores local basado en la caricatura de Tom y Jerry.

---

## 🎯 Visión del juego

Dos jugadores comparten un mismo teclado (o gamepads) controlando a **Tomás** (el gato, grande y fuerte) y **Jaimito** (el ratón, pequeño y ágil). Deben cooperar para resolver puzles de plataforma y alcanzar la meta de cada nivel.

---

## 📊 Estado actual del proyecto

| Área | Estado | Observación |
|------|--------|-------------|
| Motor | Unity 2D + URP + Cinemachine | Funcional |
| Jugadores | `TomMove.cs` y `JerryMove.cs` | Movimiento idéntico, sin diferenciación |
| Entrada | Mezcla de `Input.GetKey` y Input System | No soporta bien 2 jugadores locales por dispositivo |
| Escenas | `Menú.unity`, `Game.unity` | `Game` casi vacía (sin plataformas, sin meta) |
| Animaciones | Jerry tiene 5 clips; Tom no tiene clips visibles | Falta animación de Tom |
| UI | Botones Jugar / Salir / Opciones | "Opciones" sin función |
| Audio | Ausente | Sin música ni efectos de sonido |
| Arte | 2 hojas de sprites + fondo genérico | Necesitan recortarse y diferenciarse visualmente |

---

## 🗺️ Fases de desarrollo

### Fase 1 — Fundamentos cooperativos ✅ DESARROLLADA

**Objetivo:** Que ambos jugadores puedan moverse por un escenario básico con cámara que los siga a ambos.

**Entregables creados:**
- [x] `Assets/Scripts/Core/GameManager.cs` — manejador de estados (Menú, Playing, LevelCompleted, GameOver) y spawn de jugadores.
- [x] `Assets/Scripts/Characters/CharacterMovement.cs` — clase base con movimiento, salto y detección de suelo robusta (`Physics2D.OverlapCircle`).
- [x] `Assets/Scripts/Characters/TomMovement.cs` — Tom: más lento, salta menos, más pesado (controles WASD).
- [x] `Assets/Scripts/Characters/JerryMovement.cs` — Jaimito: más rápido, salta más alto, más ligero (controles Flechas).
- [x] `Assets/Scripts/Camera/CoopCamera.cs` — cámara que sigue al punto medio entre ambos con zoom dinámico por distancia.

**Pendiente en el Editor de Unity (configuración manual):**
- [ ] Convertir a Tom y Jaimito en **Prefabs** (arrastrarlos a `Assets/Prefabs/`).
- [ ] Crear plataformas y suelo en `Game.unity` (sprites o Tilemap).
- [ ] Crear capa `Ground` y asignarla a las plataformas.
- [ ] Añadir `GameManager` a un objeto vacío en la escena y asignar prefabs + puntos de spawn.
- [ ] Añadir `CoopCamera` a la cámara principal y asignar Tom y Jaimito como targets.
- [ ] Ajustar `orthographicSize` de la cámara (valor actual 200 es excesivo; probar entre 5 y 10 según escala de sprites).
- [ ] Retirar o deshabilitar los scripts antiguos `TomMove.cs` y `JerryMove.cs` para evitar conflictos.

**Criterio de aceptación:** Dos jugadores pueden caminar y saltar sobre plataformas en la misma escena sin pisarse el código, y la cámara los sigue a ambos.

---

### Fase 2 — Mecánicas de puzle cooperativo (2-3 semanas)

**Objetivo:** Introducir elementos que obliguen a los dos jugadores a colaborar.

- [ ] **Botón de presión dual:** Dos botones en el suelo que deben mantenerse presionados simultáneamente por Tom y Jaimito para abrir una puerta.
- [ ] **Túnel pequeño:** Hueco por el que solo Jaimito puede pasar para activar un interruptor al otro lado y abrirle el paso a Tom.
- [ ] **Puerta cerrada:** Se abre solo cuando ambos botones están activos.
- [ ] **Objetivo de nivel:** Meta común (queso o salida) que requiere que ambos jugadores lleguen juntos.
- [ ] **Cajas / objetos empujables:** Tom puede empujar cajas pesadas; Jaimito puede arrastrar objetos ligeros o queso como cebo.

**Criterio de aceptación:** Existe al menos un nivel donde ninguno de los dos puede completar el puzle sin la ayuda del otro.

---

### Fase 3 — Diferenciación de personajes (1 semana)

**Objetivo:** Que Tom y Jaimito se sientan distintos, como en la caricatura.

| Característica | Tom (Gato) | Jaimito (Ratón) |
|----------------|------------|-----------------|
| Tamaño | Grande | Pequeño |
| Velocidad | Más lenta | Más rápida |
| Salto | Más bajo, más pesado | Más alto y ágil |
| Habilidad especial | Activar botones pesados, derribar obstáculos | Cabe en túneles, arrastrar objetos ligeros |
| Vida | Más resistente | Más frágil, pero más ágil para esquivar |

- [ ] Ajustar valores de `moveSpeed`, `jumpForce` y gravedad por personaje.
- [ ] Añadir collider de cabeza/cuerpo diferenciado para que Jaimito quepa en espacios donde Tom no.
- [ ] Feedback visual distinto (color, contorno, sombra) para que sea fácil distinguirlos en pantalla.

---

### Fase 4 — Pulido y presentación (1-2 semanas)

**Objetivo:** Que el juego se vea, se oiga y se sienta completo.

#### Arte y escenografía
- [ ] Recortar las hojas de sprites en sprites individuales (`Sprite Mode = Multiple`) y crear un `Sprite Atlas`.
- [ ] Crear tileset de escenario estilo casa (suelos, paredes, muebles, alfombras, zócalos, agujeros de pared para Jaimito).
- [ ] Añadir decoración y fondos que recuerden al mundo de Tom y Jerry.
- [ ] Diseñar al menos 3 niveles progresivamente más difíciles.

#### UI y UX
- [ ] Pantalla de controles al inicio (WASD para Tom, Flechas para Jaimito).
- [ ] HUD en juego: indicador de quién es Tom (izquierda) y quién es Jaimito (derecha).
- [ ] Pantalla de victoria: "¡Nivel completado!" con opción de siguiente nivel o volver al menú.
- [ ] Pantalla de Game Over: "Reintentar nivel" o "Volver al menú".
- [ ] Arreglar botón "Opciones" del menú (volumen, pantalla completa).

#### Audio
- [ ] Música de fondo alegre y tensa al estilo caricatura.
- [ ] SFX: salto, caída, activar botón, puerta abriéndose, recoger queso, sonido de "ouch" al morir, victoria.
- [ ] Sonidos distintivos para cada personaje.

#### Feedback visual
- [ ] Partículas al saltar, caer o activar mecanismos.
- [ ] Efectos visuales al morir y al completar el nivel.

---

## 🛠️ Estructura de carpetas sugerida

```
Assets/
├── Scenes/
│   ├── Menú.unity
│   ├── Game.unity
│   └── Nivel01.unity, Nivel02.unity...
├── Scripts/
│   ├── Core/
│   │   ├── GameManager.cs
│   │   └── LevelManager.cs
│   ├── Characters/
│   │   ├── CharacterMovement.cs (base)
│   │   ├── TomMovement.cs
│   │   └── JerryMovement.cs
│   ├── Puzzles/
│   │   ├── PressureButton.cs
│   │   ├── Door.cs
│   │   └── Tunnel.cs
│   ├── Camera/
│   │   └── CoopCamera.cs
│   └── UI/
│       ├── MenuSystem.cs
│       └── HUD.cs
├── Sprites/
│   ├── Tom/
│   ├── Jerry/
│   ├── Tiles/
│   └── UI/
├── Animations/
│   ├── Tom/
│   └── Jerry/
├── Audio/
│   ├── Music/
│   └── SFX/
└── Prefabs/
    ├── Tom.prefab
    └── Jerry.prefab
```

---

## ✅ Criterios de definición de "finido" (MVP)

1. Dos jugadores pueden jugar en el mismo teclado (WASD vs Flechas).
2. Existe al menos un nivel completo con plataforma, puzle cooperativo y meta.
3. Tom y Jaimito tienen habilidades diferenciadas que son necesarias para avanzar.
4. Cámara sigue a ambos jugadores simultáneamente.
5. Menú principal funcional con Jugar, Opciones y Salir.
6. Pantalla de victoria y Game Over implementadas.
7. Música y efectos de sonido básicos presentes.

---

## 📝 Notas adicionales

- Mantener los nombres en español para coherencia con el proyecto (`Tomás`, `Jaimito`, `Jugar`, `Salir`).
- Usar control de versiones (git) y hacer commit al final de cada fase.
- Probar siempre con dos teclados o dos gamepads si se dispone de ellos, para validar la experiencia local cooperativa real.

---

*Documento generado como pauta de desarrollo. Revisar y actualizar al completar cada fase.*
