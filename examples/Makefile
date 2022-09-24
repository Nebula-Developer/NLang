NL := nlang

SRCS := $(wildcard *.nl)
OBJS := $(SRCS:.nl=)

.PHONY: all rebuild

all: $(OBJS)

rebuild:
	$(MAKE) -C ..

%: %.nl
	@echo "Compiling $<..."
	$(NL) $< -o out/$@