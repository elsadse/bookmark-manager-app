import iconSwitchVertical from "@/assets/images/icon-switch-vert.svg"
import iconFrontEndMentor from "@/assets/images/favicon-frontend-mentor.png"
import iconLeading from "@/assets/images/icon-leading.svg"
import iconVisitCount from "@/assets/images/icon-visit-count.svg"
import iconTime from "@/assets/images/icon-time.svg"
import iconDate from "@/assets/images/icon-date.svg"
import iconPin from "@/assets/images/icon-pin.svg"

export function BookmarkList({ listTitle, isBookmarkAichived, isBookmarkPin }: { listTitle: string, isBookmarkAichived: boolean, isBookmarkPin: boolean }) {
    const logoBookmark = iconFrontEndMentor
    const nameBookmark = "Frontend Mentor"
    const textSupportingNameBookmark = "frontendmentor.io"
    const descriptionBookmark = "Improve your front-end coding skills by building real projects. Solve real-world HTML, CSS and JavaScript challenges whilst working to professional designs."
    const tagsBookmark = ["Practice", "Learning", "Community"]
    const numberVisitBookmark = 47
    const dayVisitBookmark = "23 Sep"
    const dayLastedVisitBookmark = "15 Jan"

    return (
        <div className="flex flex-col gap-y-5 px-4 pt-6 pb-16">
            <div className="flex flex-row justify-between items-center gap-x-4">
                <span className="text-preset-2 text-neutral-900">{listTitle}</span>
                <button className="flex justify-center items-center gap-x-4 px-3 py-2.5 bg-neutral-0 rounded-8 border border-neutral-400">
                    <img src={iconSwitchVertical} alt="icon switch" />
                    <span className="text-preset-3 text-neutral-900">Sort by</span>
                </button>
            </div>
            <div className="grid grid-cols-1 md:grid-cols-2 xl:grid-cols-3 gap-8 pb-8 overflow-y-auto">
                <BookmarkListCard descriptionBookmark={descriptionBookmark}
                    logoBookmark={logoBookmark} nameBookmark={nameBookmark}
                    dayLastedVisitBookmark={dayLastedVisitBookmark} numberVisitBookmark={numberVisitBookmark}
                    dayVisitBookmark={dayVisitBookmark} isBookmarkAichived={isBookmarkAichived} isBookmarkPin={isBookmarkPin}
                    tagsBookmark={tagsBookmark} textSupportingNameBookmark={textSupportingNameBookmark}
                />
                <BookmarkListCard descriptionBookmark={descriptionBookmark}
                    logoBookmark={logoBookmark} nameBookmark={nameBookmark}
                    dayLastedVisitBookmark={dayLastedVisitBookmark} numberVisitBookmark={numberVisitBookmark}
                    dayVisitBookmark={dayVisitBookmark} isBookmarkAichived={isBookmarkAichived} isBookmarkPin={isBookmarkPin}
                    tagsBookmark={tagsBookmark} textSupportingNameBookmark={textSupportingNameBookmark}
                /><BookmarkListCard descriptionBookmark={descriptionBookmark}
                    logoBookmark={logoBookmark} nameBookmark={nameBookmark}
                    dayLastedVisitBookmark={dayLastedVisitBookmark} numberVisitBookmark={numberVisitBookmark}
                    dayVisitBookmark={dayVisitBookmark} isBookmarkAichived={isBookmarkAichived} isBookmarkPin={isBookmarkPin}
                    tagsBookmark={tagsBookmark} textSupportingNameBookmark={textSupportingNameBookmark}
                /><BookmarkListCard descriptionBookmark={descriptionBookmark}
                    logoBookmark={logoBookmark} nameBookmark={nameBookmark}
                    dayLastedVisitBookmark={dayLastedVisitBookmark} numberVisitBookmark={numberVisitBookmark}
                    dayVisitBookmark={dayVisitBookmark} isBookmarkAichived={isBookmarkAichived} isBookmarkPin={isBookmarkPin}
                    tagsBookmark={tagsBookmark} textSupportingNameBookmark={textSupportingNameBookmark}
                /><BookmarkListCard descriptionBookmark={descriptionBookmark}
                    logoBookmark={logoBookmark} nameBookmark={nameBookmark}
                    dayLastedVisitBookmark={dayLastedVisitBookmark} numberVisitBookmark={numberVisitBookmark}
                    dayVisitBookmark={dayVisitBookmark} isBookmarkAichived={isBookmarkAichived} isBookmarkPin={isBookmarkPin}
                    tagsBookmark={tagsBookmark} textSupportingNameBookmark={textSupportingNameBookmark}
                />
            </div>
        </div>
    )
}

type BookmarkListCardProps = {
    logoBookmark: string,
    nameBookmark: string,
    textSupportingNameBookmark: string,
    descriptionBookmark: string,
    tagsBookmark: string[],
    numberVisitBookmark: number,
    dayLastedVisitBookmark: string,
    dayVisitBookmark: string,
    isBookmarkPin: boolean,
    isBookmarkAichived: boolean,
}

export function BookmarkListCard({
    logoBookmark,
    dayLastedVisitBookmark,
    descriptionBookmark,
    dayVisitBookmark,
    nameBookmark,
    numberVisitBookmark,
    tagsBookmark,
    textSupportingNameBookmark,
    isBookmarkAichived,
    isBookmarkPin
}: BookmarkListCardProps) {

    return (
        <div className="rounded-8 bg-neutral-0">
            <BookmarkListCardContainer descriptionBookmark={descriptionBookmark} logoBookmark={logoBookmark} nameBookmark={nameBookmark} tagsBookmark={tagsBookmark} textSupportingNameBookmark={textSupportingNameBookmark} />
            <div className="flex flex-row justify-between items-center gap-x-2 px-4 py-3 border-t border-neutral-300">
                <div className="flex flex-row gap-x-4">
                    <BookmarkListCardFooterInfo icon={iconVisitCount} information={numberVisitBookmark.toString()} />
                    <BookmarkListCardFooterInfo icon={iconTime} information={dayVisitBookmark} />
                    <BookmarkListCardFooterInfo icon={iconDate} information={dayLastedVisitBookmark} />
                </div>
                {isBookmarkPin && <img src={iconPin} className="size-4" alt="icon pin" />}
                {isBookmarkAichived &&
                    <span className="text-center text-preset-5 text-neutral-800 bg-neutral-100 rounded-4 px-2 py-0.5">Archived</span>
                }
            </div>
        </div>
    )
}

type BookmarkListCardContainerProps = {
    logoBookmark: string,
    nameBookmark: string,
    textSupportingNameBookmark: string,
    descriptionBookmark: string,
    tagsBookmark: string[]
}

export function BookmarkListCardContainer({ logoBookmark, nameBookmark, textSupportingNameBookmark, descriptionBookmark, tagsBookmark }: BookmarkListCardContainerProps) {

    return (
        <div className="flex flex-col gap-y-4 p-4 rounded-10">
            <div className="flex flex-row justify-between gap-x-3">
                <div className="flex flex-row">
                    <div className="size-11 flex items-center rounded-8 border border-neutral-100">
                        <img src={logoBookmark} className="w-11 h-11" alt="icon logo" />
                    </div>
                    <div className="flex flex-col gap-1">
                        <span className="text-preset-2 text-neutral-900">{nameBookmark}</span>
                        <span className="text-preset-5 text-neutral-800">{textSupportingNameBookmark}</span>
                    </div>
                </div>
                <div className="flex justify-center items-center size-8 gap-x-1 rounded-8 bg-neutral-0 border border-neutral-400">
                    <img src={iconLeading} className="size-5" alt="icon logo" />
                </div>
            </div>
            <div className="h-px bg-neutral-300" />
            <span className="text-preset-4-md text-neutral-800 text-left">
                {descriptionBookmark}
            </span>
            <div className="flex flex-row gap-x-2">
                {tagsBookmark.map((tag, index) => (
                    <span key={index} className="text-center text-preset-5 text-neutral-800 bg-neutral-100 rounded-4 px-2 py-0.5">{tag}</span>
                ))}
            </div>
        </div>
    )
}

export function BookmarkListCardFooterInfo({ icon, information }: { icon: string, information: string }) {

    return (
        <div className="flex flex-row justify-center items-center gap-x-1.5">
            <img src={icon} className="size-3" alt="icon" />
            <span className="text-preset-5 text-left text-neutral-800">{information}</span>
        </div>
    )
}