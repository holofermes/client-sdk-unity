using UnityEngine;
using LiveKit.Proto;
using UnityEngine.Rendering;
using Unity.Collections;
using UnityEngine.Experimental.Rendering;
using System;

namespace LiveKit
{
    public class TextureVideoSource : RtcVideoSource
    {
        TextureFormat _textureFormat;
        private readonly VideoRotation _rotation;

        public Texture Texture { get; }

        public override int GetWidth()
        {
            return Texture.width;
        }

        public override int GetHeight()
        {
            return Texture.height;
        }

        protected override VideoRotation GetVideoRotation()
        {
            return _rotation;
        }

        public TextureVideoSource(Texture texture, VideoBufferType bufferType = VideoBufferType.Rgba, VideoRotation rotation = VideoRotation._0) : base(VideoStreamSource.Texture, bufferType)
        {
            Texture = texture;
            _rotation = rotation;
            base.Init();
        }

        ~TextureVideoSource()
        {
            Dispose(); 
        }

        // Read the texture data into a native array asynchronously
        protected override bool ReadBuffer()
        {
            if (_reading)
                return false;
            _reading = true;
            var textureChanged = false;

            if (_dest == null || _dest.width != GetWidth() || _dest.height != GetHeight()) {
                var compatibleFormat = SystemInfo.GetCompatibleFormat(Texture.graphicsFormat, FormatUsage.ReadPixels);
                _textureFormat = GraphicsFormatUtility.GetTextureFormat(compatibleFormat);
                _bufferType = GetVideoBufferType(_textureFormat);
                _data = new NativeArray<byte>(GetWidth() * GetHeight() * GetStrideForBuffer(_bufferType), Allocator.Persistent);
                _dest = new Texture2D(GetWidth(), GetHeight(), _textureFormat, false);
                textureChanged = true;
            }
            Graphics.CopyTexture(Texture, _dest);
            AsyncGPUReadback.RequestIntoNativeArray(ref _data, _dest, 0, _textureFormat, OnReadback);
            return textureChanged;
        }
    }
}

