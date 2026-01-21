import iconSwitchVertical from "@/assets/images/icon-switch-vert.svg"
import iconFrontEndMentor from "@/assets/images/favicon-frontend-mentor.png"
import iconLeading from "@/assets/images/icon-leading.svg"
import iconVisitCount from "@/assets/images/icon-visit-count.svg"
import iconTime from "@/assets/images/icon-time.svg"
import iconDate from "@/assets/images/icon-date.svg"
import iconPin from "@/assets/images/icon-pin.svg"

export function BookmarkList({ listTitle }: { listTitle: string }) {

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
                <BookmarkListCard />
                <BookmarkListCard />
                <BookmarkListCard />
                <BookmarkListCard />
                <BookmarkListCard />
                <BookmarkListCard />
                <BookmarkListCard />
            </div>
        </div>
    )
}

type BookmarkListCardProps={

}

export function BookmarkListCard() {

    return (
        <div className="rounded-8 bg-neutral-0">
            <BookmarkListCardContainer />
            <div className="flex flex-row justify-between items-center gap-x-2 px-4 py-3 border-t-1 border-neutral-300">
                <div className="flex flex-row gap-x-4">
                    <BookmarkListCardFooterInfo icon={iconVisitCount} information="47" />
                    <BookmarkListCardFooterInfo icon={iconTime} information="23 Sep" />
                    <BookmarkListCardFooterInfo icon={iconDate} information="15 Jan" />
                </div>
                <img src={iconPin} className="size-4" alt="icon pin" />
            </div>
        </div>
    )
}

export function BookmarkListCardContainer() {

    return (
        <div className="flex flex-col gap-y-4 p-4 rounded-10">
            <div className="flex flex-row justify-between gap-x-3">
                <div className="flex flex-row">
                    <div className="size-11 flex items-center rounded-8 border border-neutral-100">
                        <img src={iconFrontEndMentor} className="w-11 h-11" alt="icon logo" />
                    </div>
                    <div className="flex flex-col gap-1">
                        <span className="text-preset-2 text-neutral-900">Frontend Mentor</span>
                        <span className="text-preset-5 text-neutral-800">frontendmentor.io</span>
                    </div>
                </div>
                <div className="flex justify-center items-center size-8 gap-x-1 rounded-8 bg-neutral-0 border border-neutral-400">
                    <img src={iconLeading} className="size-5" alt="icon logo" />
                </div>
            </div>
            <div className="h-px bg-neutral-300" />
            <span className="text-preset-4-md text-neutral-800 text-left">
                Improve your front-end coding skills by building real projects. Solve real-world HTML, CSS and JavaScript challenges whilst working to professional designs.
            </span>
            <div className="flex flex-row gap-x-2">
                <span className="text-center text-preset-5 text-neutral-800 bg-neutral-100 rounded-4 px-2 py-0.5">Pratice</span>
                <span className="text-center text-preset-5 text-neutral-800 bg-neutral-100 rounded-4 px-2 py-0.5">Learning</span>
                <span className="text-center text-preset-5 text-neutral-800 bg-neutral-100 rounded-4 px-2 py-0.5">Community</span>
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