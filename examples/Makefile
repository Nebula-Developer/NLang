NL := nlang

SRCS := $(wildcard *.nl)
OBJS := $(SRCS:.nl=)

.PHONY: all
all: $(OBJS)

%: %.nl
	$(NL) $< -o $@