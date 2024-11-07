using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ApplicationMode {
    Manipulate,
    Edit,
    Interact

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

public enum ShapeBaseFormat {
    Undefined,
    Square,
    Triangle
}

public enum EditSolidOption {
    ColorIndividualPaint,
    VerticesShowLetters,
    VerticesShowMarkers,
    EdgesModeOutline,
    EdgesShowMarkers,
    EdgesShowLetters,
    SurfacesHideSolid,
    EdgesOcclusion,
    EdgesGlobalOcclusion,
    PhysicsGravity,
    PhysicsCollision
}


