using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ApplicationMode {
    Manipulate,
    Edit,
    Attach,
    Dettach

}

public enum SolidEditMode {
    None,
    Color,
    Vertices,
    Edges,
    Physics,
    Learn

}

public enum EditMethod {
    None,
    Paint
}

public enum UserRole {
    Teacher,
    Student,
    Spectator

}

public enum EditSolidOption {
    ColorIndividualPaint,
    VerticesShowLetters,
    VerticesShowMarkers,
    EdgesModeOutline,
    EdgesShowMarkers,
    EdgesShowLetters,
    EdgesHideSolid,
    EdgesOcclusion,
    EdgesGlobalOcclusion,
    PhysicsGravity,
    PhysicsCollision
}


