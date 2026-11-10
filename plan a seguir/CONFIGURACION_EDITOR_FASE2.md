# 🛠️ Configuración en el Editor de Unity — Fase 2

Esta guía te lleva paso a paso por lo que debes hacer **dentro del Editor de Unity** para poner en marcha las mecánicas de puzle cooperativo.

---

## 1. Crear las etiquetas (Tags) de los personajes

Antes de usar Tunnel, LevelGoal y PushableBox, necesitas las etiquetas correctas.

1. En Unity, ve a **Edit → Project Settings → Tags and Layers**.
2. En la sección **Tags**, añade estas dos etiquetas:
   - `Tom`
   - `Jerry`
3. Selecciona el objeto de Tom en la Hierarchy y asignale la etiqueta `Tom`.
4. Selecciona el objeto de Jaimito y asignale la etiqueta `Jerry`.

> También asegúrate de que los personajes tengan la etiqueta `Player` si quieres que activen los `PressureButton` (por defecto el script busca `"Player"`, pero puedes cambiarlo en el Inspector de cada botón).

---

## 2. Configurar un nivel de prueba con puzle cooperativo

Diseña un escenario donde **ninguno de los dos pueda avanzar solo**.

### Ejemplo de nivel básico:

```
[Tom]   [Botón 1]   [Puerta]   [Meta]
                                      ▲
[Jerry] [Botón 2]   [Túnel]   ────────┘
```

### Pasos:

#### A. Crear los botones de presión

1. Crea dos objetos vacíos llamados `Boton1` y `Boton2`.
2. A cada uno añade un `BoxCollider2D` con **Is Trigger** activado.
3. Añade el componente `PressureButton`.
4. En el Inspector de cada botón:
   - `Player Tag`: `Player` (o `Tom` / `Jerry` si prefieres).
   - `Hold Time`: `0` (activación instantánea al pisar).
   - `Listeners`: deja vacío por ahora (se asigna desde la puerta).

#### B. Crear la puerta

1. Crea un objeto llamado `Puerta` con un sprite de puerta.
2. Añade un `BoxCollider2D` (sin trigger) para bloquear el paso.
3. Añade el componente `Door`.
4. En el Inspector:
   - `Required Buttons`: `2` (ambos deben pisar).
   - `Open Speed`: `3`.
   - `Open Distance`: altura suficiente para que pasen (ej. `2.5`).
   - `Closed Sprite` / `Open Sprite`: asigna los sprites si los tienes.

#### C. Vincular botones con la puerta

1. Selecciona `Boton1`.
2. En el Inspector de `PressureButton`, en el campo `Listeners` (tamaño 1), arrastra el objeto `Puerta`.
3. Repite con `Boton2`.

> Ahora, cuando Tom pise Boton1 y Jaimito pise Boton2 al mismo tiempo, la puerta se abrirá. Si uno se levanta, la puerta se cierra.

#### D. Crear el túnel pequeño

1. Crea un objeto llamado `Tunel` con un sprite de agujero o túnel.
2. Añade un `BoxCollider2D` con **Is Trigger** activado.
3. Añade el componente `Tunnel`.
4. En el Inspector:
   - `Allowed Tag`: `Jerry`.
   - `Blocked Tag`: `Tom`.
5. Coloca el túnel en un pasillo por donde solo Jaimito pueda pasar para llegar al Boton2 al otro lado de un muro.

#### E. Crear la meta del nivel

1. Crea un objeto vacío llamado `Meta`.
2. Añade un `BoxCollider2D` con **Is Trigger** activado.
3. Añade el componente `LevelGoal`.
4. En el Inspector:
   - `Tom Tag`: `Tom`.
   - `Jerry Tag`: `Jerry`.
5. Colócalo al final del nivel, después de la puerta.

> Cuando Tom y Jaimito estén los dos dentro del área de `Meta` al mismo tiempo, se llamará a `GameManager.Instance.CompleteLevel()`.

#### F. Crear cajas empujables (opcional)

1. Crea un objeto llamado `Caja` con un sprite de caja y un `BoxCollider2D` (sin trigger).
2. Añade un `Rigidbody2D`.
3. Añade el componente `PushableBox`.
4. En el Inspector:
   - `Box Type`: `Normal` (ambos pueden empujar), `Heavy` (solo Tom empuja bien), o `Light` (solo Jaimito).
   - `Base Push Speed`: `3`.
5. Coloca cajas para crear puzles donde se necesite bloquear o activar botones.

---

## 3. Ejemplo completo de nivel cooperativo

Aquí tienes una disposición sugerida para tu primer nivel con puzle:

| Elemento | Posición aproximada | Propósito |
|----------|-------------------|-----------|
| Tom (spawn) | Izquierda baja | Jugador 1 |
| Jaimito (spawn) | Izquierda baja | Jugador 2 |
| Botón 1 | Centro izquierda | Tom debe pisarlo |
| Botón 2 | Centro derecha (detrás de túnel) | Jaimito debe pisarlo |
| Túnel | Entre Botón 1 y Botón 2 | Solo Jaimito pasa |
| Puerta | Centro, bloqueando el camino a la meta | Se abre con ambos botones |
| Meta | Derecha alta | Ambos deben llegar juntos |
| Caja pesada | Cerca de Botón 1 | Tom puede empujarla para alcanzar algo |

---

## 4. Cómo probar la Fase 2

1. Presiona **Play** en Unity.
2. Mueve a Tom con **WASD** y a Jaimito con las **Flechas**.
3. Haz que Tom se pare en Botón 1 y Jaimito en Botón 2 (Jaimito debe pasar por el túnel para llegar al Botón 2).
4. Observa que la puerta se abre solo cuando ambos están presionando.
5. Lleva a ambos jugadores al área de `Meta`.
6. Deberías ver en la consola: `"¡Ambos jugadores llegaron a la meta!"`.

---

## 5. Solución de problemas

| Problema | Causa probable | Solución |
|----------|---------------|----------|
| La puerta no se abre | Los botones no están vinculados a la puerta | Arrastra la puerta al campo `Listeners` de cada botón |
| Tom puede pasar por el túnel | Etiqueta mal asignada | Asigna `Tom` al objeto de Tom y `Jerry` al de Jaimito |
| La meta no se activa | GameManager no está en la escena | Añade un objeto vacío con el componente `GameManager` |
| Los personajes no activan botones | Falta etiqueta `Player` o collider trigger | Asegúrate de que los personajes tengan la etiqueta correcta y el collider del botón sea Trigger |

---

## ✅ Criterio de aceptación de la Fase 2

- [ ] Existe al menos un nivel donde Tom y Jaimito deben cooperar para avanzar.
- [ ] Ninguno de los dos puede completar el nivel solo (necesitan al otro para abrir la puerta o pasar por el túnel).
- [ ] La puerta se abre solo cuando ambos botones están activos simultáneamente.
- [ ] El nivel se completa solo cuando ambos jugadores llegan a la meta juntos.

Cuando esto funcione, estarás listo para pasar a la **Fase 3** (diferenciación visual y de habilidades entre personajes).
