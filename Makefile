.PHONY: detect build-windows build-mac build-linux install-mac install-osx

OS := $(shell uname)

detect:
	@echo "Detecting OS..."
	@if [ "$(OS)" = "Darwin" ]; then \
		echo "OS is macOS"; \
		make build-mac install-mac; \
	elif [ "$(OS)" = "Linux" ]; then \
		echo "OS is Linux"; \
		make build-linux install-linux; \
	else \
		echo "OS is Windows (potentially)"; \
		make build-windows; \
		echo "You can find the executable in the build folder."; \
	fi

build-windows:
	dotnet publish -p:PublishSingleFile=true --self-contained false -r win-x64 -c Release -o build

build-linux:
	dotnet publish -p:PublishSingleFile=true --self-contained false -r linux-x64 -c Release -o build

build-mac:
	dotnet publish -p:PublishSingleFile=true --self-contained false -r osx-x64 -c Release -o build

install-mac:
	@echo "Installing nlang to /usr/local/bin: (You may need to enter your password)"
	sudo cp ./build/nlang /usr/local/bin/nlang

install-linux:
	@echo "Installing nlang to /usr/bin: (You may need to enter your password)"
	sudo cp ./build/nlang /usr/bin/nlang