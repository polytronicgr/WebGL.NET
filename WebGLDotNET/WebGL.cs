﻿using System;
using WebAssembly;

namespace WebGLDotNET
{
    public static class WebGL
    {
        private static JSObject gl;

        public static object ArrayBuffer => GetProperty("ARRAY_BUFFER");

        public static object ColorBufferBit => GetProperty("COLOR_BUFFER_BIT");

        public static object DepthTest => GetProperty("DEPTH_TEST");

        public static object ElementArrayBuffer => GetProperty("ELEMENT_ARRAY_BUFFER");

        public static object Float => GetProperty("FLOAT");

        public static object FragmentShader => GetProperty("FRAGMENT_SHADER");

        public static object StaticDraw => GetProperty("STATIC_DRAW");

        public static object Triangles => GetProperty("TRIANGLES");

        public static object UnsignedShort => GetProperty("UNSIGNED_SHORT");

        public static object VertexShader => GetProperty("VERTEX_SHADER");

        public static void AttachShader(object program, object shader) => Invoke("attachShader", program, shader);

        public static void BindBuffer(object target, object buffer) => Invoke("bindBuffer", target, buffer);

        public static void BufferData(object target, object srcData, object usage)
        {
            var srcDataType = srcData.GetType();
            JSObject array;
            
            if (srcDataType == typeof(float[]))
            {
                var float32Array = (JSObject)Runtime.GetGlobalObject("Float32Array");
                array = Runtime.NewJSObject(float32Array, srcData);
            }
            else if (srcDataType == typeof(ushort[]))
            {
                var uint16Array = (JSObject)Runtime.GetGlobalObject("Uint16Array");
                array = Runtime.NewJSObject(uint16Array, srcData);
            }
            else
            {
                throw new NotImplementedException();
            }
            
            Invoke("bufferData", target, array, usage);
        }

        public static void Clear(object mask) => Invoke("clear", mask);

        public static void ClearColor(double red, double green, double blue, double alpha) =>
            Invoke("clearColor", red, green, blue, alpha);

        public static void CompileShader(object shader) => Invoke("compileShader", shader);

        public static object CreateBuffer() => Invoke("createBuffer");

        public static object CreateProgram() => Invoke("createProgram");

        public static object CreateShader(object type) => Invoke("createShader", type);

        public static void DrawElements(object mode, int count, object type, int offset) =>
            Invoke("drawElements", mode, count, type, offset);

        public static void Enable(object cap) => Invoke("enable", cap);

        public static void EnableVertexAttribArray(object index) => Invoke("enableVertexAttribArray", index);

        public static object GetAttribLocation(object program, string name) => 
            Invoke("getAttribLocation", program, name);

        public static void Init(JSObject canvas)
        {
            gl = (JSObject)canvas.Invoke("getContext", "webgl");
        }

        public static void LinkProgram(object program) => Invoke("linkProgram", program);

        public static void ShaderSource(object shader, string source) => Invoke("shaderSource", shader, source);

        public static void UseProgram(object program) => Invoke("useProgram", program);

        public static void VertexAttribPointer(
            object index, 
            int size, 
            object type, 
            bool normalized, 
            int stride, 
            int offset) =>
            Invoke("vertexAttribPointer", index, 3, type, normalized, stride, offset);

        public static void Viewport(int x, int y, object width, object height) => 
            Invoke("viewport", x, y, width, height);

        private static object GetProperty(string name) => gl.GetObjectProperty(name);

        private static object Invoke(string method, params object[] args) => gl.Invoke(method, args);
    }
}
