NL := nlang

SRCS := $(wildcard *.nl)
OBJS := $(SRCS:.nl=)

.PHONY: all
all: $(OBJS)

%: %.nl
	@echo "Compiling $<..."
	$(NL) $< -o out/$@
	$(NL) -c $< -o c/$@