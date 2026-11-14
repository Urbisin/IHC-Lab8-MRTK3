# Lab 8 - MRTK3 Realidad Mixta (IHC)

Contenido: proyectos para los 3 labs (Explorando MRTK3, Primera App, Lector de QR).
El video solo muestra los proyectos ya armados.

## Antes de ir (requisitos de la PC del lab)

- **Unity 2022.3 LTS** con el modulo **Universal Windows Platform Build Support**.
  Ojo: las 2022.3 muy nuevas (p. ej. 2022.3.76f1) son "Extended LTS" y no abren con licencia
  Personal. Usar una 2022.3 normal (la guia recomienda 2022.3.8f1; probado con 2022.3.50f1).
- **git** instalado (Unity baja algunos paquetes desde GitHub).
- Internet (MRTK3 ~360 MB + paquetes de Unity).
- En el HoloLens: app **Holographic Remoting Player** (Microsoft Store), misma red WiFi que la PC.
- **Clonar en una ruta corta** (p. ej. `C:\Lab8`). Con rutas largas Windows falla con
  "The filename or extension is too long" en un plugin de audio.

## Al llegar al laboratorio (en orden)

1. Clonar este repo en una ruta corta y ejecutar en PowerShell, dentro de la carpeta:
   `powershell -ExecutionPolicy Bypass -File .\setup-lab.ps1`
   (clona MRTK3 rama `mrtk3` al lado de los proyectos; tarda ~1 min).
2. Unity Hub > Add > Add project from disk, y agregar los 3 proyectos:
   - `MixedRealityToolkit-Unity\UnityProjects\MRTKDevTemplate` (Lab Explorando)
   - `MRTKDemo01` (Lab 1)
   - `LectorQR` (Lab 2)
   Si pide otra version, elegir la 2022.3 instalada y aceptar.
3. Abrir cada proyecto (la primera importacion tarda 5-15 min; abrirlos de a uno).
   Si pregunta por el nuevo Input System backend: **Yes**. Si pregunta por XR InteractionLayerMask: **No thanks**.
4. En MRTKDemo01 y LectorQR, menu **Lab8** de Unity, en este orden:
   1. `Lab8 > Configurar proyecto para HoloLens 2` (OpenXR en Standalone y UWP, perfiles Eye Gaze /
      Microsoft Hand / Motion Controller, perfil MRTK, Input System, capacidades WebCam/Microphone).
      Si Unity pide reiniciar, aceptar.
   2. MRTKDemo01: `Lab8 > Lab1 - Construir escena CameraRigScena`
      LectorQR:   `Lab8 > Lab2 - Construir escena LectorQRScene`
5. Revisar rapido en Edit > Project Settings > XR Plug-in Management > pestana **UWP** (Windows)
   que OpenXR y "Microsoft HoloLens feature group" esten marcados (esta parte no se pudo probar
   fuera del lab porque la PC de prueba no tenia el modulo UWP). Si falta, marcarlo a mano.
6. Para usar el HoloLens: Mixed Reality > Remoting > **Holographic Remoting for Play Mode**,
   poner la IP que muestra el Remoting Player en el visor, "Enable Holographic Remoting for Play Mode", y Play.

## Que grabar (guion corto, ~10 min total)

- **Explorando MRTK3** (MRTKDevTemplate > Assets/Scenes): abrir y dar Play a 5-6 escenas:
  BoundsControlExamples, InteractableButtonExamples, HandMenuExamples, SolverExamples, TapToPlaceExample.
- **Primera App** (MRTKDemo01, escena CameraRigScena): Play, mostrar Slate y clic al boton (cambia el texto).
  Sin HoloLens tambien funciona con el MRTK Input Simulator (mano simulada con teclado/mouse).
- **Lector QR** (LectorQR, escena LectorQRScene): con HoloLens (Remoting) escanear un QR
  generado en https://www.the-qrcode-generator.com/ y mostrar el contenido en el dialogo;
  luego manipular el cubo (mover, escalar con Bounds Control).
  El QR **solo** funciona con el HoloLens real. Para probar la UI sin visor: clic derecho en el
  componente TestQRCodeDetection > "Simular lectura de QR".

## Estado de las pruebas (hechas en Unity 2022.3.50f1, sin HoloLens)

- MRTKDemo01: compila, la configuracion y la escena se generan sin errores (Rig, simulador, Slate, boton + script).
- LectorQR: compila, escena generada con todas las referencias asignadas (XROrigin, ARMarkerManager,
  prefab QRCodeMarker, dialogo, cubo con ObjectManipulator + BoundsControl + MinMaxScaleConstraint).
  Usa **Mixed Reality OpenXR 1.11.2** (registro de Microsoft): la 1.7.0 que trae MRTK3 no tiene
  la API de QR (`ARMarkerManager` existe desde 1.9.0).
- No probado: pestana UWP, conexion Remoting y lectura real de QR (requieren el lab).
- No incluido: "UGUI Input Adapter Draggable" de la tarea (opcional, se agrega a mano a `cuboparent`).
