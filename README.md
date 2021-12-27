# 📂 Szechuan Memory

Provides low-level primitives to write and read bytes efficiently into underlying memory. Aims for low allocations and a good API.

The library handles most primitive values like differently sized integers, `struct`s and enums. It's centered around the possible byte orders _little endian_ and _big endian_. The underlying memory is accessed using the dotnet `Span` API to lower memory allocations and aim to work as efficient as possible while providing a simple, good usable API.


## Why not use JSON, Protobuf, …?

One can't always define and design his/her own network protocol or file format. We often have to stick to existing interfaces or implement some specification which consists of raw binary data. That may be some file format, a network protocol or some other representation of data in memory. This library aims for this exact cases: You have to read or write some simple primitive values but don't want to handle all those nitty-gritty byte conversion your own.


## Why even care about endianness?

Endianness is the order of bytes in the computers memory. Big endian stores the most significant byte at the smallest memory address and the least significant byte at the largest. Little endian system stores the least-significant byte at the smallest address.

For example lets have a look on the representation of the int32 value `100` in the two representations:

| endianness | byte 0 | byte 1 | byte 2 | byte 3 |
|------------|--------|--------|--------|--------|
| big        | `0x00` | `0x00` | `0x00` | `0x64` |
| little     | `0x64` | `0x00` | `0x00` | `0x00` |

Our x86 CPUs (and most ARM CPUs) are using little endian internally to manage its memory. In contrast, big endian is used in networking like a load of internat protocols and many familar file formats like PNG and JPEG. Different file formats are choosing either. So there's the processor architecture order and the network order and one has to use them interchangely.

The dotnet library has the `BitConverter` class to convert between primitive types and its byte representations. But that class is using the processors byte order, as one can check using the `BitConverter.IsLittleEndian` property. One has to write its own implementation for the other one or work with ugly reversal of memory.

This library handles endianness as a first class citizen and provides implementations for both byte orders.


## How do I know the endianness of my file format, protocol, spec, …?

Hopefully somebody documented this essential property, e.g. as taken from the [NBT format definition](https://minecraft.fandom.com/wiki/NBT_format#TAG_definition): _…, followed by a two byte **big-endian** unsigned integer for the length of the name…_


## Managing memory

This library doesn't manage memory on its own but provides stream-like functionality on top of allocated memory blocks. One may new-up a byte array and use a `ByteWriter` to write primitive values into it. Or one may read some bytes from a socket or file and read primitive values from there.

You are the owner of the memory, the library is only providing a view onto it.


## Reading primitive values

Open your memory in a byte reader and start reading. The reader will keep track of the current position.

```c#
var memory = new byte[1000]; // This can come from anywhere like from a File.Read(...) or a socket
var reader = ByteReader.Open(memory, Endian.Little);
var length = reader.ReadUShort();
var type = reader.ReadEnum<TagType>();
var name = reader.Read(length);
var chunk = reader.ReadStruct<ChunkRef>();
// ...
```


## Writing primitive values

Open your memory in a byte writer and start writing. The writer will keep track of the current position.

```c#
var memory = new byte[1000]; // Pre-allocate some appropriatly sized chunk of memory
var writer = ByteWriter.Open(memory, Endian.Little);
var nameBytes = Encoding.ASCII.GetBytes(name);
writer
    .Write(nameBytes.Length)
    .Write(TagType.Acronym)
    .Write(nameBytes)
    .WriteStruct(chunk);
// ...
```
