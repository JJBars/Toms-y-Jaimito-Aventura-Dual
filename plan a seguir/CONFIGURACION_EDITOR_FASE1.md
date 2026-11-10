# 🛠️ Configuración en el Editor de Unity — Fase 1

Esta guía te lleva paso a paso por lo que debes hacer **dentro del Editor de Unity** para poner en marcha la Fase 1.

---

## 1. Crear la carpeta de Prefabs

En la ventana **Project**, haz clic derecho en `Assets/` → **Create → Folder** y nómbrala `Prefabs`.

---

## 2. Convertir a Tom y Jaimito en Prefabs

1. En la escena `Game.unity`, busca los objetos de **Tom** y **Jaimito** (o créalos si no existen).
2. A cada uno, añade los componentes correspondientes:
   - **Tom:** `Rigidbody2D` + `TomMovement` + un `Collider2D` (BoxCollider2D o CapsuleCollider2D).
   - **Jaimito:** `Rigidbody2D` + `JerryMovement` + un `Collider2D`.
3. A cada objeto, añade un objeto hijo llamado `GroundCheck`:
   - Crea un GameObject vacío hijo del personaje.
   - Posiciónalo un poco abajo del centro (por ejemplo `Y = -0.5`).
   - Asócialo al campo `Ground Check` del script `CharacterMovement`.
4. Arrastra cada objeto desde la **Hierarchy** a la carpeta `Assets/Prefabs/` para crear los prefabs.
   - Ahora puedes eliminar los originales de la escena (el `GameManager` los instanciará al iniciar).

---

## 3. Crear la capa "Ground"

1. En el Inspector de cualquier plataforma, busca el desplegable **Layer** → **Add Layer...**
2. Añade una nueva capa llamada `Ground`.
3. Selecciona todas tus plataformas y suelo, y asignales la capa `Ground`.
4. En el script `CharacterMovement` de Tom y Jaimito, asigna esa misma capa al campo `Ground Layer`.

---

## 4. Crear plataformas y suelo en `Game.unity`

La escena actual está casi vacía. Crea un escenario básico:

1. **Suelo:** Crea un objeto vacío llamado `Suelo`, añade un `SpriteRenderer` con un sprite de plataforma, un `BoxCollider2D` y asigna la capa `Ground`.
2. **Plataformas:** Duplica el suelo y colócalo a distintas alturas para crear saltos.
3. **Paredes laterales (opcional):** Para evitar que los jugadores se salgan de la pantalla.

> Consejo: usa `Tilemap` si prefieres trabajar con baldosas, o sprites sueltos si es más rápido para prototipar.

---

## 5. Configurar el GameManager

1. En la escena `Game.unity`, crea un objeto vacío llamado `GameManager`.
2. Añade el componente `GameManager`.
3. En el Inspector:
   - Arrastra el prefab de **Tom** al campo `Tom Prefab`.
   - Arrastra el prefab de **Jaimito** al campo `Jerry Prefab`.
   - Crea dos objetos vacíos llamados `TomSpawnPoint` y `JerrySpawnPoint`, colócalos donde quieras que inicien cada uno, y arrástralos a sus respectivos campos.
4. Deja el objeto `GameManager` en la escena (no necesitas convertirlo en prefab, gracias al `DontDestroyOnLoad`).

---

## 6. Configurar la cámara cooperativa

1. Selecciona la **Main Camera** de la escena.
2. Añade el componente `CoopCamera`.
3. En el Inspector:
   - `Target Tom`: arrastra el objeto de Tom (o su prefab si ya está en la escena).
   - `Target Jerry`: arrastra el objeto de Jaimito.
   - `Min Orthographic Size`: `5`
   - `Max Orthographic Size`: `14`
   - `Smooth Speed`: `5`
4. **Importante:** en el componente `Camera` de la Main Camera, cambia `Orthographic Size` a `5` (el valor actual de `200` hace que todo se vea diminuto).
5. Si usas Cinemachine, puedes desactivar el `CinemachineBrain` o la `CinemachineCamera` de la escena para evitar conflictos, y dejar que `CoopCamera` controle la cámara directamente.

---

## 7. Limpiar scripts antiguos

Los scripts originales `TomMove.cs` y `JerryMove.cs` todavía están en `Assets/Scripts/`. Para evitar que dos scripts controlen el mismo personaje:

- Opción A: bórralos.
- Opción B: desactiva los componentes `TomMove` y `JerryMove` en los objetos de la escena.

Los nuevos scripts (`TomMovement` y `JerryMovement`) ya heredan de `CharacterMovement` y usan controles separados (WASD vs Flechas).

---

## 8. Cómo probar la Fase 1

1. Presiona **Play** en Unity.
2. Deberías ver a Tom y Jaimito aparecer en sus puntos de spawn.
3. Controla a Tom con **WASD** y a Jaimito con las **Flechas**.
4. Ambos deben poder caminar y saltar sobre las plataformas.
5. La cámara debe seguir al punto medio entre ellos y hacer zoom si se separan.

Si algo no funciona, revisa la consola de Unity (`Window → General → Console`) para ver errores de referencia nula o capas mal asignadas.

---

## ✅ Criterio de aceptación de la Fase 1

- [ ] Dos jugadores se mueven simultáneamente en la misma escena.
- [ ] Cada uno usa su propio teclado (WASD para Tom, Flechas para Jaimito).
- [ ] Ambos pueden saltar sobre plataformas sin atravesarlas.
- [ ] La cámara sigue a ambos y hace zoom dinámico.
- [ ] El `GameManager` puede cambiar entre estados (Menú → Jugando).

Cuando esto funcione, estarás listo para pasar a la **Fase 2** (mecánicas de puzle cooperativo).
